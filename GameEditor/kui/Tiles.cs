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

		private int _tilesAcross = 0;
		private int _tilesDown = 0;

		public Tiles(Chunk tileChunk, ISurface image)
		: base(tileChunk, image)
		{
			Zoom = 2;
		}

		private int Divide { get { return (TileSize + Margin) * Zoom; } }

		private int TileWidth => Zoom * ((_tilesAcross * 9)+1);
		private int TileHeight => Zoom * ((_tilesDown * 9)+1);

		public ISurface Draw()
		{
			Image.BeginDraw();

			_tilesAcross = 32;
			_tilesDown = NumberOfTiles / _tilesAcross;

			int w = TileWidth;
			int h = TileHeight;
			Image.Create(w, h);
			Image.Fill(new Rgba(0.5f, 0.5f, 0.5f, 1.0f));
			TileDrawer.Zoom = Zoom;
			
			int	index = 0;
			int sy = Divide;
			int sx = Divide;

			for(int y = 1; y < h - Zoom; y+=sy)
			{
				for(int x = 1; x < w - Zoom; x+=sx)
				{
					TileDrawer.Draw(x, y, index++, Image);
				}
			}

			Image.EndDraw();
			return Image;
		}

		public void DrawTile(int index)
		{
			int row = index / _tilesAcross;
			int col = index % _tilesAcross;

			TileDrawer.Draw(
				col * Divide,
				row * Divide,
				index,
				Image);
		}

        public int PointToTile(int x, int y)
        {
			int margin = Margin * Zoom;

            return ( (x - margin)/ Divide) + 
				_tilesAcross * 
				((y - margin) / Divide);
        }
	}
}
