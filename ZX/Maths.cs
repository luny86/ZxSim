
namespace ZX
{

    static public class Maths
    {
        static public int Bit8_Signed(byte value)
        {
            return Bit8_Signed((int)value);
        }

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