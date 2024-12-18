
namespace KUtil
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
    }
}