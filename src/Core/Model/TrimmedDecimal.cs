using System.Numerics;

namespace ProgrammingWithPalermo.ChurchBulletin.Core.Model
{
    public class TrimmedDecimal : IComparable
    {
        
        public decimal Value { get; set; }

        public TrimmedDecimal(decimal? val)
        {
            Value = val.GetValueOrDefault();
        }

        public override string ToString()
        {
            return Value.ToString("0.######");
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is TrimmedDecimal)
            {
                return this.CompareTo(obj) == 0;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (obj is TrimmedDecimal)
            {
                return this.CompareTo((TrimmedDecimal) obj);
            }

            throw new Exception($"Can't compare to instance of {obj.GetType()}");
        }
        public int CompareTo(TrimmedDecimal other)
        {
            if (other == null)
            {
                return 1;
            }
            if (this.Value < other.Value)
            {
                return -1;
            }
            if (this.Value > other.Value)
            {
                return 1;
            }
            return 0;
        }

        public static bool operator ==(TrimmedDecimal left, TrimmedDecimal right)
        {
            if ((object) left == null)
            {
                return (object) right == null;
            }

            return left.Equals(right);
        }
        public static bool operator !=(TrimmedDecimal left, TrimmedDecimal right)
        {
            return !(left == right);
        }
        public static bool operator >(TrimmedDecimal left, TrimmedDecimal right)
        {
            if (left == null)
            {
                return false;
            }

            return left.CompareTo(right) > 0;
        }
        public static bool operator <(TrimmedDecimal left, TrimmedDecimal right)
        {
            if (left == null)
            {
                return right != null;
            }

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

        public static implicit operator decimal(TrimmedDecimal num) => num == null ? 0 : num.Value;
        public static implicit operator float(TrimmedDecimal num) => num == null ? 0 : (float)num.Value;
        public static implicit operator int(TrimmedDecimal num) => num == null ? 0 : (int)num.Value;
        public static implicit operator TrimmedDecimal(decimal num) => new(num);
        public static implicit operator TrimmedDecimal(float num) => new((decimal)num);
        public static implicit operator TrimmedDecimal(int num) => new(num);
    }
}
