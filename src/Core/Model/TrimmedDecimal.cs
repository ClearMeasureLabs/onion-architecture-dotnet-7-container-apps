namespace ProgrammingWithPalermo.ChurchBulletin.Core.Model;

public readonly struct TrimmedDecimal : IComparable
{
    public TrimmedDecimal(decimal? val)
    {
        Value = val.GetValueOrDefault();
    }

    private decimal Value { get; }

    public int CompareTo(object? obj)
    {
        if (obj == null) return 1;
        if (obj is TrimmedDecimal trimmedDecimal) return CompareTo(trimmedDecimal);

        throw new Exception($"Can't compare to instance of {obj.GetType()}");
    }

    public override string ToString()
    {
        return Value.ToString("0.######");
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is TrimmedDecimal) return CompareTo(obj) == 0;
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public int CompareTo(TrimmedDecimal other)
    {
        if (Value < other.Value) return -1;
        if (Value > other.Value) return 1;
        return 0;
    }

    public static bool operator ==(TrimmedDecimal left, TrimmedDecimal right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TrimmedDecimal left, TrimmedDecimal right)
    {
        return !(left == right);
    }

    public static bool operator >(TrimmedDecimal left, TrimmedDecimal right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <(TrimmedDecimal left, TrimmedDecimal right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >=(TrimmedDecimal left, TrimmedDecimal right)
    {
        return left > right || left == right;
    }

    public static bool operator <=(TrimmedDecimal left, TrimmedDecimal right)
    {
        return left < right || left == right;
    }

    public static bool operator ==(TrimmedDecimal left, decimal right)
    {
        return left == new TrimmedDecimal(right);
    }

    public static bool operator !=(TrimmedDecimal left, decimal right)
    {
        return left != new TrimmedDecimal(right);
    }

    public static bool operator >(TrimmedDecimal left, decimal right)
    {
        return left > new TrimmedDecimal(right);
    }

    public static bool operator <(TrimmedDecimal left, decimal right)
    {
        return left < new TrimmedDecimal(right);
    }

    public static bool operator >=(TrimmedDecimal left, decimal right)
    {
        return left >= new TrimmedDecimal(right);
    }

    public static bool operator <=(TrimmedDecimal left, decimal right)
    {
        return left <= new TrimmedDecimal(right);
    }

    //=========================================
    public static bool operator ==(decimal left, TrimmedDecimal right)
    {
        return new TrimmedDecimal(left) == right;
    }

    public static bool operator !=(decimal left, TrimmedDecimal right)
    {
        return new TrimmedDecimal(left) != right;
    }

    public static bool operator >(decimal left, TrimmedDecimal right)
    {
        return new TrimmedDecimal(left) > right;
    }

    public static bool operator <(decimal left, TrimmedDecimal right)
    {
        return new TrimmedDecimal(left) < right;
    }

    public static bool operator >=(decimal left, TrimmedDecimal right)
    {
        return new TrimmedDecimal(left) >= right;
    }

    public static bool operator <=(decimal left, TrimmedDecimal right)
    {
        return new TrimmedDecimal(left) <= right;
    }

    public static implicit operator decimal(TrimmedDecimal num)
    {
        return num.Value;
    }

    public static implicit operator float(TrimmedDecimal num)
    {
        return (float)num.Value;
    }

    public static implicit operator int(TrimmedDecimal num)
    {
        return (int)num.Value;
    }

    public static implicit operator TrimmedDecimal(decimal num)
    {
        return new(num);
    }

    public static implicit operator TrimmedDecimal(float num)
    {
        return new((decimal)num);
    }

    public static implicit operator TrimmedDecimal(int num)
    {
        return new(num);
    }
}