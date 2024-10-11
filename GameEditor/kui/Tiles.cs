using KUtil;
using Platform;

namespace KUi
{
	/// <summary>
	/// Draws tiles from a memory chunk
	/// </summary>
	public class Tiles : TileBase
	{
		private const int TileSize = 8;
		private const int Margin = 1;

		private int _zoom = 2;
		private int _tilesAcross = 0;
		private int _tilesDown = 0;

		public Tiles(Chunk tileChunk, ISurface image)
		: base(tileChunk, image)
		{
		}

		private int Divide { get { return (TileSize + Margin) * _zoom; } }

		public ISurface Draw()
		{
			int numTiles = TileChunk.Length / 8;
			_tilesAcross = 32;
			_tilesDown = numTiles / _tilesAcross;

			int w = _zoom * ((_tilesAcross * 9)+1);
			int h = _zoom * ((_tilesDown * 9)+1);
			Image.Create(w, h);
			Image.Fill(new Rgba(0.5f, 0.5f, 0.5f, 1.0f));
			
			int	nextByte = 0;
			int sy = Divide;
			int sx = Divide;

			for(int y = 1; y < h - _zoom; y+=sy)
			{
				for(int x = 1; x < w - _zoom; x+=sx)
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

        public int PointToTile(int x, int y)
        {
			int margin = Margin * _zoom;

            return ( (x - margin)/ Divide) + 
				_tilesAcross * 
				((y - margin) / Divide);
        }
	}
}
