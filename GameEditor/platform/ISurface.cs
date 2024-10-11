using KUtil;
using System;

namespace Platform
{
    public interface ISurface
    {
        event EventHandler Updated;

        void BeginDraw();
        void EndDraw();
        void Create(int w, int h);
        void Fill(Rgba colour);
        void FillRect(Rectangle rect, Rgba colour);
        void SetPixel(int x, int y, Rgba colour);
    }
}
