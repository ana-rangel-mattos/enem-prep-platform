using NpgsqlTypes;

namespace EnemPrep.Domain.Enums;

public enum UserRole
{
    [PgName("STUDENT")] Student = 0,
    [PgName("ADMIN")] Admin = 1
}