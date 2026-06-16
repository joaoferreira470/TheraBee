using System.Globalization;

namespace Patients.Application.Reports.Services;

public class ReportDraftBuilder(IApplicationDbContext dbContext, IPatientProgressDashboardBuilder dashboardBuilder) : IReportDraftBuilder
{
    private static readonly CultureInfo PtPt = CultureInfo.GetCultureInfo("pt-PT");

    public async Task<Report> BuildAsync(Guid patientId, Guid therapistId, DateTime periodStart, DateTime periodEnd, CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(patient => patient.Id == PatientId.Of(patientId) && patient.TherapistId == therapistId, cancellationToken);

        if (patient == null)
        {
            throw new PatientNotFoundException(patientId);
        }

        var patientDto = patient.ToPatientDto();

        var sessions = await dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.SessionGoals)
            .Where(session => session.PatientId == patientId && session.TherapistId == therapistId)
            .OrderBy(session => session.StartDateTime)
            .ToListAsync(cancellationToken);

        var filteredSessions = sessions
            .Where(session => session.StartDateTime >= periodStart && session.StartDateTime <= periodEnd)
            .ToList();

        var goals = await dbContext.TherapeuticGoals
            .AsNoTracking()
            .Where(goal => goal.PatientId == patientId && goal.TherapistId == therapistId)
            .OrderBy(goal => goal.Type)
            .ThenBy(goal => goal.Description)
            .ToListAsync(cancellationToken);

        var dashboard = await dashboardBuilder.BuildAsync(patientId, therapistId, cancellationToken, periodStart, periodEnd);
        var title = $"Relatório de Progresso Terapêutico - {patient.Name}";

        var patientSnapshot = BuildPatientSnapshot(patientDto);
        var executiveSummary = BuildExecutiveSummary(patientDto, dashboard, periodStart, periodEnd);
        var attendanceSummary = BuildAttendanceSummary(dashboard);
        var goalProgressSummary = BuildGoalProgressSummary(goals, dashboard);
        var sessionSummary = BuildSessionSummary(filteredSessions, dashboard);
        var recommendations = BuildRecommendations(filteredSessions, patientDto);

        return Report.CreateDraft(
            id: Guid.NewGuid(),
            patientId: patient.Id.Value,
            therapistId: therapistId,
            title: title,
            periodStart: periodStart,
            periodEnd: periodEnd,
            patientSnapshot: patientSnapshot,
            executiveSummary: executiveSummary,
            attendanceSummary: attendanceSummary,
            goalProgressSummary: goalProgressSummary,
            sessionSummary: sessionSummary,
            recommendations: recommendations,
            additionalNotes: null);
    }

    private static string BuildPatientSnapshot(PatientDto patient)
    {
        var ageText = patient.CalculatedAge > 0 ? $"{patient.CalculatedAge} anos" : "idade indisponível";
        var caregiverText = string.IsNullOrWhiteSpace(patient.CaregiverName)
            ? "Cuidador: não registado"
            : $"Cuidador: {patient.CaregiverName}";

        return string.Join(Environment.NewLine, new[]
        {
            $"Paciente: {patient.Name}",
            $"Idade: {ageText}",
            $"Diagnóstico principal: {patient.MainDiagnosis}",
            $"Género: {patient.Gender ?? "Não especificado"}",
            $"Telefone: {patient.PhoneNumber ?? "Não especificado"}",
            $"Email: {patient.Email ?? "Não especificado"}",
            caregiverText,
            $"Motivo de referência: {patient.ReferralReason ?? "Não especificado"}",
            $"Notas gerais: {patient.GeneralNotes ?? "Sem notas gerais"}"
        });
    }

    private static string BuildExecutiveSummary(PatientDto patient, PatientProgressDashboardDto dashboard, DateTime periodStart, DateTime periodEnd)
    {
        return string.Join(Environment.NewLine, new[]
        {
            $"Este relatório acompanha {patient.Name} no período entre {periodStart.ToString("dd MMM yyyy", PtPt)} e {periodEnd.ToString("dd MMM yyyy", PtPt)}.",
            $"Neste período foram concluídas {dashboard.Attendance.CompletedSessions} sessões num total de {dashboard.Attendance.TotalSessions} sessões registadas.",
            $"Estão definidos {dashboard.Goals.TotalPresets} presets terapêuticos: {dashboard.Goals.Areas} áreas e {dashboard.Goals.Objectives} objetivos."
        });
    }

    private static string BuildAttendanceSummary(PatientProgressDashboardDto dashboard)
    {
        return string.Join(Environment.NewLine, new[]
        {
            $"Sessões concluídas: {dashboard.Attendance.CompletedSessions}",
            $"Sessões canceladas: {dashboard.Attendance.CancelledSessions}",
            $"Faltas: {dashboard.Attendance.NoShowSessions}",
            $"Sessões futuras: {dashboard.Attendance.UpcomingSessions}",
            $"Sessões pendentes de registo: {dashboard.Attendance.PendingRegistrationSessions}",
            $"Taxa de presença: {dashboard.Attendance.AttendanceRate:0.#}%"
        });
    }

    private static string BuildGoalProgressSummary(IEnumerable<TherapeuticGoal> goals, PatientProgressDashboardDto dashboard)
    {
        var goalList = goals.Any()
            ? string.Join(", ", goals.Select(goal => $"{LocalizeGoalType(goal.Type.ToString())}: {goal.Description}"))
            : "Ainda não existem objetivos registados.";

        return string.Join(Environment.NewLine, new[]
        {
            $"Total de presets: {dashboard.Goals.TotalPresets}",
            $"Áreas: {dashboard.Goals.Areas}",
            $"Objetivos: {dashboard.Goals.Objectives}",
            $"Presets trabalhados: {goalList}"
        });
    }

    private static string BuildSessionSummary(IEnumerable<Session> sessions, PatientProgressDashboardDto dashboard)
    {
        var lastSessionText = dashboard.LastSession == null
            ? "Ainda não existe sessão anterior concluída ou realizada."
            : $"Última sessão: {dashboard.LastSession.StartDateTime.ToString("dd MMM yyyy HH:mm", PtPt)} - {LocalizeSessionType(dashboard.LastSession.Type.ToString())} - {LocalizeSessionStatus(dashboard.LastSession.Status.ToString())}";

        var nextSessionText = dashboard.NextSession == null
            ? "Não existe próxima sessão agendada."
            : $"Próxima sessão: {dashboard.NextSession.StartDateTime.ToString("dd MMM yyyy HH:mm", PtPt)} - {LocalizeSessionType(dashboard.NextSession.Type.ToString())} - {dashboard.NextSession.Location}";

        var recentSessions = sessions
            .OrderByDescending(session => session.StartDateTime)
            .Take(5)
            .Select(session => $"{session.StartDateTime.ToString("dd MMM yyyy HH:mm", PtPt)} - {LocalizeSessionType(session.Type.ToString())} - {LocalizeSessionStatus(session.Status.ToString())}");

        return string.Join(Environment.NewLine, new[]
        {
            lastSessionText,
            nextSessionText,
            "Sessões recentes:",
            string.Join(Environment.NewLine, recentSessions.DefaultIfEmpty("Sem sessões registadas.")),
            string.Empty
        });
    }

    private static string BuildRecommendations(IEnumerable<Session> sessions, PatientDto patient)
    {
        var latestRecommendation = sessions
            .OrderByDescending(session => session.StartDateTime)
            .Select(session => session.Recommendations)
            .FirstOrDefault(recommendation => !string.IsNullOrWhiteSpace(recommendation));

        if (!string.IsNullOrWhiteSpace(latestRecommendation))
        {
            return latestRecommendation!;
        }

        return string.Join(Environment.NewLine, new[]
        {
            $"Continuar a trabalhar o plano clínico atual de {patient.Name}.",
            "Rever os objetivos ativos na próxima sessão agendada.",
            "Registar checkpoints sessão a sessão para que o próximo relatório possa ser gerado mais rapidamente."
        });
    }

    private static string LocalizeSessionStatus(string status)
    {
        return status switch
        {
            "Scheduled" => "agendada",
            "Rescheduled" => "reagendada",
            "Completed" => "concluída",
            "Cancelled" => "cancelada",
            "NoShow" => "falta",
            _ => status
        };
    }

    private static string LocalizeSessionType(string type)
    {
        return type switch
        {
            "Assessment" => "avaliação",
            "Intervention" => "intervenção",
            "FollowUp" => "seguimento",
            _ => type
        };
    }
    private static string LocalizeGoalType(string type)
    {
        return type switch
        {
            "Area" => "area",
            "Objective" => "objetivo",
            _ => type
        };
    }
}
