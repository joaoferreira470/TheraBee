using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Patients.Application.Reports.Models;
using Patients.Application.Services;

namespace Patients.Infrastructure.AI;

public class OpenAiClinicalReportNarrativeGenerator(
    HttpClient httpClient,
    IOptions<OpenAiOptions> options,
    ILogger<OpenAiClinicalReportNarrativeGenerator> logger) : IClinicalReportNarrativeGenerator
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ClinicalReportNarrative?> GenerateAsync(ClinicalReportDraftContext context, CancellationToken cancellationToken)
    {
        var openAiOptions = options.Value;
        if (string.IsNullOrWhiteSpace(openAiOptions.ApiKey))
        {
            return null;
        }

        ConfigureClient(openAiOptions);

        var request = new
        {
            model = openAiOptions.Model,
            temperature = 0.2,
            response_format = new { type = "json_object" },
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = BuildSystemPrompt()
                },
                new
                {
                    role = "user",
                    content = BuildUserPrompt(context)
                }
            }
        };

        using var response = await httpClient.PostAsJsonAsync("chat/completions", request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("OpenAI report generation failed with status {StatusCode}: {Body}", response.StatusCode, responseBody);
            return null;
        }

        var content = ExtractMessageContent(responseBody);
        if (string.IsNullOrWhiteSpace(content))
        {
            logger.LogWarning("OpenAI report generation returned an empty response body.");
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<ClinicalReportNarrative>(content, JsonSerializerOptions);
        }
        catch (JsonException exception)
        {
            logger.LogWarning(exception, "OpenAI report generation returned malformed JSON.");
            return null;
        }
    }

    private void ConfigureClient(OpenAiOptions openAiOptions)
    {
        if (httpClient.BaseAddress == null)
        {
            httpClient.BaseAddress = new Uri(openAiOptions.BaseUrl.TrimEnd('/') + "/");
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiOptions.ApiKey);
    }

    private static string BuildSystemPrompt()
    {
        return """
És um assistente clínico especializado em relatórios de terapia/psicomotricidade em português de Portugal.

Regras obrigatórias:
- Usa apenas os dados fornecidos.
- Não inventes informação clínica.
- Não mudes datas, scores, nomes ou observações.
- Não uses inglês.
- Mantém tom profissional, claro e útil para registo clínico.
- Responde apenas em JSON válido.
- Preenche exatamente as chaves pedidas.
- Se um campo não tiver informação suficiente, escreve uma formulação prudente e explícita.

Devolve um JSON com as chaves:
{
  "executiveSummary": "...",
  "attendanceSummary": "...",
  "goalProgressSummary": "...",
  "sessionSummary": "...",
  "recommendations": "...",
  "additionalNotes": "..."
}
""";
    }

    private static string BuildUserPrompt(ClinicalReportDraftContext context)
    {
        var payload = new
        {
            patient = new
            {
                name = context.Patient.Name,
                age = context.Patient.CalculatedAge,
                diagnosis = context.Patient.MainDiagnosis,
                gender = context.Patient.Gender,
                caregiver = context.Patient.CaregiverName,
                referralReason = context.Patient.ReferralReason,
                generalNotes = context.Patient.GeneralNotes
            },
            period = new
            {
                start = context.PeriodStart.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                end = context.PeriodEnd.ToString("yyyy-MM-ddTHH:mm:ssZ")
            },
            dashboard = new
            {
                attendance = context.Dashboard.Attendance,
                goals = context.Dashboard.Goals,
                lastSession = context.Dashboard.LastSession,
                nextSession = context.Dashboard.NextSession
            },
            goals = context.Goals.Select(goal => new
            {
                id = goal.Id,
                type = goal.Type.ToString(),
                description = goal.Description,
                priority = goal.Priority.ToString()
            }),
            sessions = context.Sessions
                .OrderBy(session => session.StartDateTime)
                .Select(session => new
                {
                    id = session.Id,
                    start = session.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    end = session.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    type = session.Type.ToString(),
                    status = session.Status.ToString(),
                    location = session.Location,
                    clinicalSummary = session.ClinicalSummary,
                    objectivesWorked = session.ObjectivesWorked,
                    progressRating = session.ProgressRating,
                    activities = session.Activities,
                    patientResponse = session.PatientResponse,
                    difficulties = session.Difficulties,
                    recommendations = session.Recommendations,
                    nextSteps = session.NextSteps,
                    assessments = session.SessionGoalAssessments
                        .OrderBy(assessment => assessment.CreatedAt ?? DateTime.MinValue)
                        .Select(assessment => new
                        {
                            goalId = assessment.TherapeuticGoalId,
                            score = assessment.Score,
                            notes = assessment.ClinicalNotes
                        })
                })
        };

        var serialized = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return $"""
Gera um rascunho de relatório clínico com base nestes dados em JSON:
{serialized}

Preenche as secções do relatório de forma cronológica e coerente.
""";
    }

    private static string? ExtractMessageContent(string responseBody)
    {
        using var document = JsonDocument.Parse(responseBody);
        if (document.RootElement.TryGetProperty("choices", out var choices) &&
            choices.ValueKind == JsonValueKind.Array &&
            choices.GetArrayLength() > 0)
        {
            var message = choices[0].GetProperty("message");
            if (message.TryGetProperty("content", out var content))
            {
                return content.GetString();
            }
        }

        return null;
    }
}
