using System;
using ZX.Util;
using GameEditorLib.Platform;

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
			TileChunk = tileChunk;
			Zoom = PixelScale + Margin;
		}

		private IChunk TileChunk { get; }

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
			if(index <0 || index >= NumberOfTiles)
			{
				throw new ArgumentException("Tile index out of range");
			}

			Draw();
		}

		private void Draw()
		{
			Image.BeginDraw();
			// Convert into tile offset

			CreateImage();

			TileDrawer.Gap = Margin;
			TileDrawer.Zoom = Zoom;
			TileDrawer.Draw(Margin, Margin, TileIndex, Image);
			
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
