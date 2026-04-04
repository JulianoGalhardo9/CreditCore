using Identity.Domain.Enums;

namespace Identity.Domain.Entities;
public class User
{
    public Guid Id { get; private set; }

    public string FullName { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }

    public bool IsActive { get; private set; }
    public User(string fullName, string email, string passwordHash, Role role)
    {
        Id = Guid.NewGuid(); 
        
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        
        Role = role;
        
        IsActive = true; 
    }
    public void Deactivate()
    {
        IsActive = false;
    }
}