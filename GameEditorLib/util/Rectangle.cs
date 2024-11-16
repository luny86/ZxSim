
namespace KUtil
{
    /// <summary>
    /// Simple class for passing rect values into platform 
    /// objects.
    /// </summary>
    public class Rectangle
    {
        public Rectangle(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public int X { get; }
        public int Y { get; }
        public int W { get; }
        public int H { get; }

        public int Left => X;
        public int Right => X+W;
        public int Top => Y;
        public int Bottom => Y+H;
    }
}