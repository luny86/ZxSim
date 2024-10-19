using KUtil;
using Platform;

namespace KUi
{
    public class TileBase
	{

        public TileBase(IChunk tileChunk, ISurface image)
        {
            TileChunk = tileChunk;
			Image = image;
            Paper = zx.Palette.BrightYellow;
			Ink = zx.Palette.BrightBlack;
			Zoom = 2;
        }

        protected IChunk TileChunk
        {
            get;
            private set;
        }

        protected ISurface	Image { get; }


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

		protected int Zoom
		{
			get;
			set;
		}

		/// <summary>
		/// Draw a sing tile at a given position.
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="nextByte">Offset address of tile</param>
		/// <returns>Next offset address after tile.</returns>
		protected int DrawTile(int x, int y, int nextByte)
		{
			for(int row = 0; row < 8; row++)
			{
				byte b = TileChunk[nextByte++];
				DrawByte(b, x, y+(row * Zoom));
			}

			return nextByte;
		}

    	protected void DrawByte(byte b, int x, int y, int gap=0)
		{
			for(int pixel = 0; pixel<8; pixel++)
			{
				Rectangle rect = new Rectangle(x+(pixel*Zoom), y, Zoom-gap ,Zoom-gap);
				Image.FillRect(rect, 
					(b & 128) == 0 ? Paper : Ink);
				b <<= 1;
			}
		}
    }
}