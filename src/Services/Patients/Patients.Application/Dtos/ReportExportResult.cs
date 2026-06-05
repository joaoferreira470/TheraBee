namespace Patients.Application.Dtos;

public record ReportExportResult(
    byte[] Content,
    string ContentType,
    string FileName);
