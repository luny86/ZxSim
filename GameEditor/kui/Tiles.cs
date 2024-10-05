using Godot;
using System;
using KUtil;

namespace KUi
{
	/// <summary>
	/// Draws tiles from a emory chunk
	/// </summary>
	public class Tiles : TileBase
	{
		private int _zoom = 2;

		public Tiles(Chunk tileChunk)
		: base(tileChunk)
		{
		}

		public Image Draw()
		{
			int numTiles = TileChunk.Length / 8;
			int tilesAcross = 32;
			int tilesDown = (numTiles / tilesAcross);

			int w = _zoom * ((tilesAcross * 9)+1);
			int h = _zoom * ((tilesDown * 9)+1);
			Image = Image.Create(w, h, true, Image.Format.Rgba8);
			Image.Fill(new Color(0.5f, 0.5f, 0.5f, 1.0f));
			
			int	nextByte = 0;
			int sy = 9 * _zoom;
			int sx = 9 * _zoom;

			for(int y = 1; y < h - _zoom; y+=sy)
			{
				for(int x = 1; x < w; x+=sx)
				{
					for(int row = 0; row < 8; row++)
					{
						byte b = TileChunk[nextByte++];
						DrawByte(b, x, y+(row * 2), 2);
					}
				}
			}

			return Image;
		}
	}
}
