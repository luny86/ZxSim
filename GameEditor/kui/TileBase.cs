using KUtil;
using Platform;
using GameEditorLib.Platform;

namespace KUi
{
	public class TileBase
	{

		public TileBase(IChunk tileChunk, ISurface image)
		{
			NumberOfTiles = tileChunk.Length / 8;
			TileDrawer = new TileDrawer(tileChunk);
			Image = image;
		}

		protected int NumberOfTiles
		{
			get;
		}

		protected TileDrawer TileDrawer
		{
			get;
		}

		protected ISurface	Image { get; }

		protected int Zoom
		{
			get;
			set;
		}
	}
}
