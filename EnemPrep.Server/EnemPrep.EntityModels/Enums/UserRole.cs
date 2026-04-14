using NpgsqlTypes;

namespace EnemPrep.EntityModels.Enums;

public enum UserRole
{
    [PgName("STUDENT")] Student = 0,
    [PgName("ADMIN")] Admin = 1
}