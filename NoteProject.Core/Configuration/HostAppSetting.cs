
namespace NoteProject.Core.Configuration;

public class HostAppSetting
{
    public static HostAppSetting Instance { get; private set; }
    public static void SetUpInstance(HostAppSetting instance) => Instance = instance;

    public const string SECTION_NAME = "HostAppSetting";

    public double SessionTimeout { get; set; }

    public string SiteUrl { get; set; }
}
