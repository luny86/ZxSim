
using ZX.Platform;
using ZX.Util;

namespace ZX.Drawing
{
    internal class BitmapDrawer : IDrawer, IAttribute
    {
        private IChunk _data = null!;
        private int _width;
        private int _height;

        /// <summary>
        /// Converts 8 bit data into a 2 colour bitmap
        /// </summary>
        /// <param name="bitmapData">Chunk of memory holding the original 8 bit bitmap data.</param>
        /// <param name="width">Width of bitmap.</param>
        /// <param name="height">Height of bitmap.</param>
        public BitmapDrawer(IChunk bitmapData, int width, int height)
        {
            _data = bitmapData;
            _width = width;
            _height = height;
        }

        Rgba IAttribute.Paper { get; set; } = Palette.Transparent;
        Rgba IAttribute.Ink { get; set; } = Palette.Yellow;

        void IDrawer.Draw(ISurface surface, int index, int x, int y)
        {
            int offset = 0;
            Rgba ink = (this as IAttribute).Ink;
            surface.Fill((this as IAttribute).Paper);

            for(int r = 0; r < _height; r++)
            {
                for(int c = 0; c < _width; c+=8)
                {
                    int m = 128;
                    int b = _data[offset++];

                    for(int bit = 0; bit < 8; bit++)
                    {
                        if(c+bit > _width)
                            break;

                        if((b & m) != 0)
                        {
                            surface.SetPixel(x+c+bit, r, ink);
                        }

                        m >>=1;
                    }
                }
            }
        }
    }
}