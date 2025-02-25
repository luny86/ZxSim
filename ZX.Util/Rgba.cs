
namespace ZX.Util
{
    /// <summary>
    /// Basic RGBA structure for conversions.
    /// </summary>
    public class Rgba
    {
        public Rgba()
        {

        }
        
        public Rgba(float r, float g, float b, float a=1.0f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public float R { get; }
        public float G { get; }
        public float B { get; }
        public float A { get; }

        public override int GetHashCode()
        {
            return R.GetHashCode() ^
                G.GetHashCode() ^
                B.GetHashCode() ^
                A.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if(obj is null)
            {
                return false;
            }
            else if(obj is Rgba rgba)
            {
                return R == rgba.R &&
                    G == rgba.G &&
                    B == rgba.B &&
                    A == rgba.A;
            }
            else
            {
                return false;
            }
        }
        static public bool operator==(Rgba lvalue, Rgba rvalue)
        {
            return lvalue.Equals(rvalue);
        }

        static public bool operator!=(Rgba lvalue, Rgba rvalue)
        {
            return !(lvalue == rvalue);
        }
    }
}