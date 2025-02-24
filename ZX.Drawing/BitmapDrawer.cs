
using ZX.Platform;
using ZX.Util;

namespace ZX.Drawing
{
    internal class BitmapDrawer : IDrawer, IAttribute, ISizeableDrawer
    {
        private IChunk _data = null!;
        private int _width;
        private int _height;

        private Rectangle _blitRect;

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
            _blitRect = new Rectangle(0,0,_width,_height);
            (this as ISizeableDrawer).PreClear = true;
        }

        Rgba IAttribute.Paper { get; set; } = Palette.Transparent;
        Rgba IAttribute.Ink { get; set; } = Palette.Yellow;

        bool ISizeableDrawer.PreClear { get; set; }

        /// <summary>
        /// Gets and sets the area that is blitted from the original image.
        /// </summary>
        public Rectangle BlitRect 
        { 
            get
            {
                return _blitRect;
            }

            set
            {
                if(value.IsEmpty)
                {
                    _blitRect = new Rectangle(0,0,_width,_height);
                }
                else
                {
                    _blitRect = value;
                }
            }
        }
        

        void IDrawer.Draw(ISurface surface, int index, int x, int y)
        {
            Rgba ink = (this as IAttribute).Ink;
            Rgba paper = (this as IAttribute).Paper;

            int h = BlitRect.H;
            int w = BlitRect.W;

            if((this as ISizeableDrawer).PreClear)
            {
                surface.FillRect(new Rectangle(x,y,_width,_height), Palette.Transparent);
            }

            int offset = index * ((_width/8)*_height)
                ;//+ ((BlitRect.Left/8) + (BlitRect.Top*(_width/8)));

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

                        if(BlitRect.InRect(c+bit, r))
                        {
                            surface.SetPixel(x+c+bit, r+y, 
                                ((b & m) == 0) ? paper : ink);
                        }

                        m >>=1;
                    }
                }
            }
        }
    }
}