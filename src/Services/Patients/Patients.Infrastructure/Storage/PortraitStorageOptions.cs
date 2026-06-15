namespace Patients.Infrastructure.Storage;

public class PortraitStorageOptions
{
    public const string SectionName = "PortraitStorage";

    public string RootPath { get; set; } = "data/portraits";
}
