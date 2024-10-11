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
        private const int TilesizeInBytes = 8;
		private const int PixelScale = 16;
		private const int TileWidth = 8;
		private const int TileHeight = 8;
		private const int Margin = 1; // In pixels (scaled)

		public TileGrid(Chunk tileChunk, ISurface image)
		: base(tileChunk, image)
		{
		}

        public int TileIndex
        {
            get;
            private set;
        }

        private static int Width 
        { 
            get
            {
                return (TileWidth * PixelScale) + (Margin * 10); 
            }
        }

        private static int Height
        {
            get
            {
                return (TileHeight * PixelScale)+ (Margin * 10); 
            }
        }

        private static int Zoom
        {
            get
            {
                return PixelScale + Margin;
            }
        }

		private void CreateImage()
		{
			Image.Create(Width, Height);
			Image.Fill(new Rgba(0.5f, 0.5f, 0.5f, 1.0f));
		}

		/// <summary>
		/// Draw a tile as a grid.
		/// </summary>
		/// <param name="index">Index of tile</param>
		/// <returns>Image containing tile.</returns>
		public void Draw(int index)
		{
            TileIndex = index;
			index *= TilesizeInBytes;
			if(index <0 || index > TileChunk.Length)
			{
				throw new ArgumentException("Tile index out of range");
			}

            Draw();
        }

        private void Draw()
        {
            int index = TileIndex*TilesizeInBytes;

			Image.BeginDraw();
			// Convert into tile offset

			CreateImage();

			int margin = Margin;
			int zoom = Zoom;

			for(int y=0;y<8; y++)
			{
				byte b = TileChunk[index++];
				DrawByte(b, margin, margin + (y*zoom), zoom, margin);
			}
			
			Image.EndDraw();
		}

        public void TogglePixel(int x, int y)
        {
            int offset = TileIndex * TilesizeInBytes;
            int row = (y-1)/ Zoom;
            int bit = 1 << (7- (x-1) / Zoom);

            byte b = TileChunk[offset+row];
            b ^= (byte)bit;
            TileChunk[offset+row] = b;

            Draw();
        }
	}
}

/*
    -
    O
    -
    O
    -
    O
    -
*/