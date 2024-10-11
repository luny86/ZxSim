using System;
using KUtil;
using Platform;

namespace KUi
{
    /// <summary>
    /// Displays an 8 bit tile as an editable grid
    /// </summary>
    public class TileGrid : TileBase
    {
        private const int PixelScale = 16;
        private const int TileWidth = 8;
        private const int TileHeight = 8;
        private const int Margin = 1; // In pixels (scaled)

        public TileGrid(Chunk tileChunk, ISurface image)
        : base(tileChunk, image)
        {
        }

        private void CreateImage()
        {
            Image.Create(
                (TileWidth * PixelScale) + (Margin * 10),
                (TileHeight * PixelScale)+ (Margin * 10));
			Image.Fill(new Rgba(0.5f, 0.5f, 0.5f, 1.0f));
        }

        /// <summary>
        /// Draw a tile as a grid.
        /// </summary>
        /// <param name="index">Index of tile</param>
        /// <returns>Image containing tile.</returns>
        public ISurface Draw(int index)
        {
            Image.BeginDraw();
            // Convert into tile offset
            index *= 8;

            if(index <0 || index > TileChunk.Length)
            {
                throw new ArgumentException("Tile index out of range");
            }

            CreateImage();

            int margin = Margin;
            int zoom = PixelScale + margin;

            for(int y=0;y<8; y++)
            {
                byte b = TileChunk[index++];
                DrawByte(b, margin, margin + (y*zoom), zoom, margin);
            }
            
            Image.EndDraw();
            return Image;
        }
    }
}