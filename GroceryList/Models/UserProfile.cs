using System.ComponentModel.DataAnnotations;

namespace GroceryList.Models;

public class UserProfile
{
    private static readonly EmailAddressAttribute _emailValidator = new();
    private string _email = string.Empty;

    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email
    {
        get => _email;
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && !_emailValidator.IsValid(value))
            {
                throw new ArgumentException("Email must be a valid email address.", nameof(value));
            }

            _email = value ?? string.Empty;
        }
    }
}
