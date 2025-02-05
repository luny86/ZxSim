
namespace ZX
{
    /// <summary>
    /// Help class for dealing with Z80 maths.
    /// </summary>
    static public class Maths
    {
        /// <summary>
        /// Converts a binary value into a signed integer.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted signed integer</returns>
        static public int Bit8_Signed(byte value)
        {
            return Bit8_Signed((int)value);
        }

        /// <summary>
        /// Converts a byte value (held in an int) into
        /// a signed integer.
        /// </summary>
        /// <param name="value">Value to convert, is rounded down to 255</param>
        /// <returns>Converted signed integer.</returns>
        static public int Bit8_Signed(int value)
        {
            int signed = value;

            if (signed > 127)
            {
                // Signed bit
                signed = -(((signed ^ 0xFF) & 0xFF) + 1);
            }

            return signed;
        }

    }
}