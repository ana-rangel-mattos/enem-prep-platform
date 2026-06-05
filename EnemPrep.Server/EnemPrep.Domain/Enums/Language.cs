using NpgsqlTypes;

namespace EnemPrep.Domain.Enums;

public enum Language
{
    [PgName("INGLES")] English = 0,
    [PgName("ESPANHOL")] Spanish = 1
}