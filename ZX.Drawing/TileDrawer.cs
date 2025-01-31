
using ZX;
using ZX.Platform;
using KUtil;

namespace ZX.Drawing
{
    /// <summary>
    /// Draws 8x8 pixel tiles onto a surface from a given set of tile graphics.
    /// </summary>
    internal class TileDrawer : IAttribute, IDrawer
    {
        public TileDrawer(IChunk tileBinaryChunk)
        {
            TileBinaryChunk = tileBinaryChunk ?? throw new ArgumentNullException(nameof(tileBinaryChunk));
            (this as IAttribute).Ink = Palette.Yellow;
            (this as IAttribute).Paper = Palette.Black;
        }

        private IChunk TileBinaryChunk
        {
            get;
        }

        #region IAttribute
        Rgba IAttribute.Paper {get; set;} = null!;
        Rgba IAttribute.Ink {get; set;} = null!;
        #endregion 

        /// <summary>
        /// Draw a single tile onto the surface.
        /// </summary>
        /// <param name="drawSurface">Surface to draw on.</param>
        /// <param name="index">Index of tile. If out of bounds, then nothing is drawn.</param>
        /// <param name="x">Position on surface in 8x8 characters.</param>
        /// <param name="y">Position on surface in 8x8 characters.</param>
        public void Draw(ISurface drawSurface, int index, int x, int y)
        {
            if(index <0 || index > (TileBinaryChunk.Length/8))
            {
                throw new ArgumentOutOfRangeException(nameof(index)); // Do nothing
            }

            int offset = index * 8;
            // Make sure the position is corrected to pixels.
            x *= 8;
            y *= 8;

            for(int row = 0; row <8; row++)
            {
                int px = x;
                int b = TileBinaryChunk[offset++];
                int m = 128;
                // Binary to pixels...
                for(int bit = 0; bit<8; bit++)
                {
                    drawSurface.SetPixel(px, y, 
                        (b & m) == 0 ? 
                            (this as IAttribute).Paper : (this as IAttribute).Ink);
                    px++;
                    m >>= 1;
                }

                y++;
            }
        }
    }
}