using NpgsqlTypes;

namespace EnemPrep.EntityModels.Enums;

public enum SubjectName
{
    [PgName("linguagens")] Languages = 0,
    [PgName("ciencias-humanas")] HumanSciences = 1,
    [PgName("ciencias-natureza")] NaturalSciences = 2,
    [PgName("matematica")] Mathematics = 3
}