using ZX.Util;

namespace ZX.Platform
{
    /// <summary>
    /// Describes a surface which wraps a platform
    /// dependent bitmap drawing object.
    /// </summary>
    public interface ISurface
    {
        event EventHandler Updated;

        void BeginDraw();
        void EndDraw();
        void Create(int w, int h);
        void Fill(Rgba colour);
        void FillRect(Rectangle rect, Rgba colour);
        void SetPixel(int x, int y, Rgba colour);

        void Blend(ISurface to, int x, int y);

        /// <summary>
        /// Test if position is within bounds of surface
        /// </summary>
        bool IsInBounds(int x, int y);
    }
}
