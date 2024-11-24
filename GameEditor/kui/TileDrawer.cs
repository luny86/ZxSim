using Platform;
using KUtil;
using GameEditorLib.Platform;
using zx = GameEditorLib.zx;

namespace KUi
{
	public class TileDrawer : IDrawer, GameEditorLib.zx.IAttribute
	{
		// TODO Create an ITileDrawer, for TileStart etc.
		// Convert references to IDrawer.
		
		public TileDrawer(IChunk tileChunk)
		{
			TileChunk = tileChunk;
			Paper = zx.Palette.BrightYellow;
			Ink = zx.Palette.BrightBlack;
			Zoom = 2;
			Gap = 0;
		}

		protected IChunk TileChunk
		{
			get;
		}

		/// <summary>
		/// Offset of first tile
		/// </summary>
		public int TileStart { get; private set;}

		public int Zoom
		{
			get;
			set;
		}

		public int Gap // Between pixels
		{
			get;
			set;
		}

		public Rgba Ink
		{
			get;
			set;
		}

		public Rgba Paper
		{
			get;
			set;
		}

		public int TileFromAddr(int address)
		{
			return (TileChunk.Start - address) / 8;
		}

		public void Draw(int x, int y, int index, ISurface image)
		{
			int offset = TileStart + (index * 8);
			for(int row = 0; row < 8; row++)
			{
				byte b = TileChunk[offset++];
				DrawByte(image, b, x, y+(row * Zoom));
			}
		}
		
	   	protected void DrawByte(ISurface image, byte b, int x, int y)
		{
			for(int pixel = 0; pixel<8; pixel++)
			{
				Rectangle rect = new Rectangle(x+(pixel*Zoom), y, Zoom-Gap ,Zoom-Gap);
				image.FillRect(rect, 
					(b & 128) == 0 ? Paper : Ink);
				b <<= 1;
			}
		}

		public void SetTileStart(int address)
		{
			TileStart = address - TileChunk.Start;
		}
	}
}
