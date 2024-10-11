using KUtil;
using Platform;

namespace KUi
{
    public class TileBase
	{

        public TileBase(Chunk tileChunk, ISurface image)
        {
            TileChunk = tileChunk;
			Image = image;
            Paper = zx.Palette.BrightYellow;
			Ink = zx.Palette.BrightBlack;

        }

        protected Chunk TileChunk
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

    	protected void DrawByte(byte b, int x, int y, int zoom = 1, int gap=0)
		{
			for(int pixel = 0; pixel<8; pixel++)
			{
				Rectangle rect = new Rectangle(x+(pixel*zoom), y, zoom-gap ,zoom-gap);
				Image.FillRect(rect, 
					(b & 128) == 0 ? Paper : Ink);
				b <<= 1;
			}
		}
    }
}