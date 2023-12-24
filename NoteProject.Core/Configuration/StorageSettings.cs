
namespace NoteProject.Core.Configuration;

public class StorageSettings
{
    public static StorageSettings Instance { get; private set; }
    public static void SetUpInstance(StorageSettings instance) => Instance = instance;

    public const string SECTION_NAME = "Storage";

    public string Provider { get; set; }

    public string LocalMediaUrl { get; set; }
}
