using Patients.Application.Reports.Models;

namespace Patients.Application.Services;

public interface IClinicalReportNarrativeGenerator
{
    Task<ClinicalReportNarrative?> GenerateAsync(ClinicalReportDraftContext context, CancellationToken cancellationToken);
}
