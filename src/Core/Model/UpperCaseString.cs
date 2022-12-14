namespace ProgrammingWithPalermo.ChurchBulletin.Core.Model;

public class UpperCaseString : IEquatable<UpperCaseString>
{
    public string? Value { get; set; }

    public UpperCaseString(string value)
    {
        Value = value?.ToUpperInvariant();
    }

    

    public static bool operator ==(UpperCaseString? left, UpperCaseString? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(UpperCaseString? left, UpperCaseString? right)
    {
        return !Equals(left, right);
    }

    public override string? ToString()
    {
        return Value;
    }

    public static implicit operator UpperCaseString(string value)
    {
        return new UpperCaseString(value);
    }

    public static implicit operator string?(UpperCaseString? value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        return value.Value;
    }

    public bool Equals(UpperCaseString? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UpperCaseString)obj);
    }

    public override int GetHashCode()
    {
        return (Value != null ? Value.GetHashCode() : 0);
    }
}