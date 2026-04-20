using EnemPrep.Domain.Primitives;

namespace EnemPrep.Domain.Models;

public sealed class Role: Enumeration<Role>
{
    public static readonly Role Student = new(1, "Student");
    public static readonly Role Admin = new(2, "Admin");
    
    public Role(int id, string name)
        : base(id, name)
    {
    }

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public ICollection<User> Users { get; set; } = new List<User>();
}