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
		private int _tilesAcross = 0;
		private int _tilesDown = 0;

		public Tiles(Chunk tileChunk)
		: base(tileChunk)
		{
		}

		private int Divide { get { return 9 * _zoom; } }

		public Image Draw()
		{
			int numTiles = TileChunk.Length / 8;
			_tilesAcross = 32;
			_tilesDown = (numTiles / _tilesAcross);

			int w = _zoom * ((_tilesAcross * 9)+1);
			int h = _zoom * ((_tilesDown * 9)+1);
			Image = Image.CreateEmpty(w, h, true, Image.Format.Rgba8);
			Image.Fill(new Color(0.5f, 0.5f, 0.5f, 1.0f));
			
			int	nextByte = 0;
			int sy = Divide;
			int sx = Divide;

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

        public int PointToTile(Vector2 position)
        {
            return ((int)position.X / Divide) + _tilesAcross * ((int)position.Y / Divide);
        }
	}
}
