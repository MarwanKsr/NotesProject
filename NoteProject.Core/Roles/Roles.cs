using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Core.Roles;

public static class Roles
{
    public const string User = "User";

    public static List<string> GetAllRoles()
    {
        var roles = typeof(Roles)
            .GetFields()
            .Select(x => x.GetValue(x).ToString())
            .ToList();

        return roles;
    }
}
