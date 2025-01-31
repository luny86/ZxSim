

namespace ZX.Util
{
    /// <summary>
    /// Holds a range for byte codes
    /// which can be tested against a given value.
    /// </summary>
    public class Range
    {
        public Range(byte from, byte lessThan)
        {
            GreaterThan = from;
            LessThan = lessThan;
        }

        byte GreaterThan { get; }
        public byte LessThan { get; }

        public bool Within(byte compare)
        {
            return compare >= GreaterThan && compare < LessThan;
        }
    }
}