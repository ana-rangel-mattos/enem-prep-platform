using NpgsqlTypes;

namespace EnemPrep.Domain.Enums;

public enum Language
{
    [PgName("INGLES")] Ingles = 0,
    [PgName("ESPANHOL")] Espanhol = 1
}