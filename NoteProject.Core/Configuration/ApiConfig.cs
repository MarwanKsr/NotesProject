using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Core.Configuration;

public class ApiConfig
{
    public const string SECTION_NAME = "ApiConfig";
    public static ApiConfig Instance { get; private set; }
    public static void SetUpInstance(ApiConfig instance) => Instance = instance;

    public string SecretKey { get; set; }

    public int KeyExpiration { get; set; }
}
