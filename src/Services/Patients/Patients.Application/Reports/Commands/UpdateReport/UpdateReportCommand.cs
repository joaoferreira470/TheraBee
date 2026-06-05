using FluentValidation;

namespace Patients.Application.Reports.Commands.UpdateReport;

public record UpdateReportCommand(Guid ReportId, UpdateReportDto Report)
    : ICommand<UpdateReportResult>;

public record UpdateReportResult(bool IsSuccess);

public class UpdateReportValidator : AbstractValidator<UpdateReportCommand>
{
    public UpdateReportValidator()
    {
        RuleFor(x => x.ReportId).NotEmpty().WithMessage("ReportId is required");
        RuleFor(x => x.Report.Title).NotEmpty().MaximumLength(200).WithMessage("Title is required");
        RuleFor(x => x.Report.PatientSnapshot).NotEmpty().MaximumLength(2000).WithMessage("PatientSnapshot is required");
        RuleFor(x => x.Report.ExecutiveSummary).NotEmpty().MaximumLength(4000).WithMessage("ExecutiveSummary is required");
        RuleFor(x => x.Report.AttendanceSummary).NotEmpty().MaximumLength(4000).WithMessage("AttendanceSummary is required");
        RuleFor(x => x.Report.GoalProgressSummary).NotEmpty().MaximumLength(4000).WithMessage("GoalProgressSummary is required");
        RuleFor(x => x.Report.SessionSummary).NotEmpty().MaximumLength(6000).WithMessage("SessionSummary is required");
        RuleFor(x => x.Report.Recommendations).NotEmpty().MaximumLength(4000).WithMessage("Recommendations is required");
        RuleFor(x => x.Report.AdditionalNotes).MaximumLength(4000);
        RuleFor(x => x.Report.PeriodEnd)
            .GreaterThanOrEqualTo(x => x.Report.PeriodStart)
            .WithMessage("PeriodEnd must be on or after PeriodStart");
    }
}
