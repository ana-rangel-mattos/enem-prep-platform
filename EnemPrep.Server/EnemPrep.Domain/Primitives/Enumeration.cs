using System.Reflection;

namespace EnemPrep.Domain.Primitives;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
where TEnum : Enumeration<TEnum>
{
    private static readonly Lazy<Dictionary<int, TEnum>> Enumerations = new (CreateEnumerations);
    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public int Id { get; protected init; }
    public string Name { get; protected init; } = string.Empty;

    public static TEnum? FromId(int value)
    {
        return Enumerations.Value.GetValueOrDefault(value);
    }

    public static TEnum? FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }
        
        return Enumerations
            .Value.Values
            .SingleOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    public static ICollection<TEnum> GetValues()
    {
        return Enumerations.Value.Values.ToList();
    }
    
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
            return false;

        return GetType() == other.GetType() && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fields = enumerationType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => fieldInfo.GetValue(null))
            .OfType<TEnum>();

        var properties = enumerationType
            .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(propertyInfo => enumerationType.IsAssignableFrom(propertyInfo.PropertyType))
            .Select(propertyInfo => propertyInfo.GetValue(null))
            .OfType<TEnum>();
        
        return fields
            .Concat(properties)
            .GroupBy(enumeration => enumeration.Id)
            .ToDictionary(group => group.Key, group => group.First());
    }
}