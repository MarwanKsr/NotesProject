using NoteProject.Core.Base;
using NoteProject.Core.Constants;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace NoteProject.Core.Domain.Identity;

public class ApplicationUser : IdentityUser, ISoftDeleteMarker, IAuditable
{
    public ApplicationUser() { }
    public ApplicationUser(string firstName, string lastName, string email)
    {
        Email = email;
        SetFirstName(firstName);
        SetLastName(lastName);
    }

    public bool IsDeleted { get; private set; }
    public DateTime DeletedAt { get; private set; }
    public string DeletedBy { get; private set; } = "";

    public string FirstName { get; private set; }

    public void SetFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            throw new ArgumentException("First name can not be empty");
        }
        firstName = firstName.Trim();

        var match = Regex.Match(firstName, RegularExpressions.ALPHABETS_AND_SPACE_ONLY, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            throw new ArgumentException("First name must be only alphabets");
        }

        FirstName = firstName;
    }

    public string LastName { get; private set; }

    public void SetLastName(string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
        {
            throw new ArgumentException("Last name can not be empty");
        }
        lastName = lastName.Trim();

        var match = Regex.Match(lastName, RegularExpressions.ALPHABETS_AND_SPACE_ONLY, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            throw new ArgumentException("Last name must be only alphabets");
        }

        LastName = lastName;
    }

    public string GetFullName() => $"{FirstName} {LastName}";

    public void SoftDelete(string deletedBy)
    {
        DeletedBy = deletedBy;
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ModifiedBy { get; private set; }

    public DateTime? ModifiedAt { get; private set; }
    public void AuditModify(string modifiedBy)
    {
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}
