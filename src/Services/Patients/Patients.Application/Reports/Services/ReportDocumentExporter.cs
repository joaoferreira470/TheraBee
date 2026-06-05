using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Patients.Application.Reports.Services;

public static class ReportDocumentExporter
{
    private const string PdfContentType = "application/pdf";
    private const string WordContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    public static ReportExportResult ExportPdf(Report report)
    {
        var lines = BuildReportLines(report);
        var fileName = BuildFileName(report.Title, "pdf");
        var content = BuildPdf(lines);

        return new ReportExportResult(content, PdfContentType, fileName);
    }

    public static ReportExportResult ExportWord(Report report)
    {
        var lines = BuildReportLines(report);
        var fileName = BuildFileName(report.Title, "docx");
        var content = BuildWord(lines, report.Title);

        return new ReportExportResult(content, WordContentType, fileName);
    }

    private static IReadOnlyList<string> BuildReportLines(Report report)
    {
        return new List<string>
        {
            report.Title,
            string.Empty,
            $"Period: {report.PeriodStart:dd MMM yyyy} to {report.PeriodEnd:dd MMM yyyy}",
            string.Empty,
            "Patient Snapshot",
            report.PatientSnapshot,
            string.Empty,
            "Executive Summary",
            report.ExecutiveSummary,
            string.Empty,
            "Attendance Summary",
            report.AttendanceSummary,
            string.Empty,
            "Goal Progress Summary",
            report.GoalProgressSummary,
            string.Empty,
            "Session Summary",
            report.SessionSummary,
            string.Empty,
            "Recommendations",
            report.Recommendations,
            string.Empty,
            "Additional Notes",
            report.AdditionalNotes ?? "No additional notes provided."
        };
    }

    private static byte[] BuildPdf(IReadOnlyList<string> lines)
    {
        var wrappedLines = WrapLines(lines, 85).ToList();
        const int linesPerPage = 44;
        var pages = wrappedLines.Chunk(linesPerPage).ToList();

        var objects = new List<string>();
        objects.Add("<< /Type /Catalog /Pages 2 0 R >>");

        var pageCount = pages.Count;
        var kids = new StringBuilder();
        var pageObjectsStart = 4;
        for (var i = 0; i < pageCount; i++)
        {
            kids.Append($"{pageObjectsStart + (i * 2)} 0 R ");
        }

        objects.Add($"<< /Type /Pages /Kids [{kids}] /Count {pageCount} >>");
        objects.Add("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");

        foreach (var page in pages)
        {
            var content = BuildPdfPageContent(page);
            objects.Add($"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 3 0 R >> >> /Contents {objects.Count + 2} 0 R >>");
            objects.Add($"<< /Length {Encoding.ASCII.GetByteCount(content)} >>\nstream\n{content}\nendstream");
        }

        return BuildPdfFile(objects);
    }

    private static string BuildPdfPageContent(IReadOnlyList<string> lines)
    {
        var sb = new StringBuilder();
        sb.AppendLine("BT");
        sb.AppendLine("/F1 12 Tf");
        sb.AppendLine("14 TL");
        sb.AppendLine("50 760 Td");

        for (var i = 0; i < lines.Count; i++)
        {
            if (i > 0)
            {
                sb.AppendLine("T*");
            }

            sb.Append('(');
            sb.Append(EscapePdf(lines[i]));
            sb.AppendLine(") Tj");
        }

        sb.AppendLine("ET");
        return sb.ToString();
    }

    private static byte[] BuildPdfFile(IReadOnlyList<string> objects)
    {
        var builder = new StringBuilder();
        builder.AppendLine("%PDF-1.4");

        var offsets = new List<int> { 0 };

        for (var i = 0; i < objects.Count; i++)
        {
            offsets.Add(Encoding.ASCII.GetByteCount(builder.ToString()));
            builder.AppendLine($"{i + 1} 0 obj");
            builder.AppendLine(objects[i]);
            builder.AppendLine("endobj");
        }

        var xrefPosition = Encoding.ASCII.GetByteCount(builder.ToString());
        builder.AppendLine("xref");
        builder.AppendLine($"0 {objects.Count + 1}");
        builder.AppendLine("0000000000 65535 f ");
        for (var i = 1; i < offsets.Count; i++)
        {
            builder.AppendLine($"{offsets[i]:0000000000} 00000 n ");
        }

        builder.AppendLine("trailer");
        builder.AppendLine($"<< /Size {objects.Count + 1} /Root 1 0 R >>");
        builder.AppendLine("startxref");
        builder.AppendLine(xrefPosition.ToString(CultureInfo.InvariantCulture));
        builder.AppendLine("%%EOF");

        return Encoding.ASCII.GetBytes(builder.ToString());
    }

    private static byte[] BuildWord(IReadOnlyList<string> lines, string title)
    {
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            CreateZipEntry(archive, "[Content_Types].xml", BuildContentTypesXml());
            CreateZipEntry(archive, "_rels/.rels", BuildRelsXml());
            CreateZipEntry(archive, "word/document.xml", BuildDocumentXml(lines, title));
            CreateZipEntry(archive, "word/styles.xml", BuildStylesXml());
        }

        return memoryStream.ToArray();
    }

    private static void CreateZipEntry(ZipArchive archive, string path, string content)
    {
        var entry = archive.CreateEntry(path, CompressionLevel.Fastest);
        using var stream = entry.Open();
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        writer.Write(content);
    }

    private static string BuildContentTypesXml()
    {
        return """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
  <Default Extension="xml" ContentType="application/xml"/>
  <Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
  <Override PartName="/word/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml"/>
</Types>
""";
    }

    private static string BuildRelsXml()
    {
        return """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
</Relationships>
""";
    }

    private static string BuildStylesXml()
    {
        return """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:styles xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:style w:type="paragraph" w:default="1" w:styleId="Normal">
    <w:name w:val="Normal"/>
    <w:qFormat/>
  </w:style>
</w:styles>
""";
    }

    private static string BuildDocumentXml(IReadOnlyList<string> lines, string title)
    {
        XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        var body = new XElement(w + "body");

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                body.Add(new XElement(w + "p", new XElement(w + "r", new XElement(w + "t", new XText(string.Empty)))));
                continue;
            }

            body.Add(new XElement(w + "p",
                new XElement(w + "r",
                    new XElement(w + "t", new XText(line)))));
        }

        body.Add(new XElement(w + "sectPr",
            new XElement(w + "pgSz", new XAttribute(w + "w", "12240"), new XAttribute(w + "h", "15840")),
            new XElement(w + "pgMar",
                new XAttribute(w + "top", "1440"),
                new XAttribute(w + "right", "1440"),
                new XAttribute(w + "bottom", "1440"),
                new XAttribute(w + "left", "1440"),
                new XAttribute(w + "header", "720"),
                new XAttribute(w + "footer", "720"),
                new XAttribute(w + "gutter", "0"))));

        var document = new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            new XElement(w + "document",
                new XAttribute(XNamespace.Xmlns + "w", w),
                body));

        return document.ToString(SaveOptions.DisableFormatting);
    }

    private static IEnumerable<string> WrapLines(IEnumerable<string> lines, int maxWidth)
    {
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                yield return string.Empty;
                continue;
            }

            if (line.Length <= maxWidth)
            {
                yield return line;
                continue;
            }

            var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var current = new StringBuilder();

            foreach (var word in words)
            {
                if (current.Length == 0)
                {
                    current.Append(word);
                    continue;
                }

                if (current.Length + 1 + word.Length <= maxWidth)
                {
                    current.Append(' ').Append(word);
                    continue;
                }

                yield return current.ToString();
                current.Clear();
                current.Append(word);
            }

            if (current.Length > 0)
            {
                yield return current.ToString();
            }
        }
    }

    private static string BuildFileName(string title, string extension)
    {
        var normalized = Regex.Replace(title.ToLowerInvariant(), @"[^a-z0-9]+", "_")
            .Trim('_');

        if (string.IsNullOrWhiteSpace(normalized))
        {
            normalized = "report";
        }

        return $"TheraBee_{normalized}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{extension}";
    }

    private static string EscapePdf(string value)
    {
        var ascii = new string(value.Select(character => character <= 0x7F ? character : '?').ToArray());

        return ascii
            .Replace(@"\", @"\\")
            .Replace("(", @"\(")
            .Replace(")", @"\)");
    }
}
