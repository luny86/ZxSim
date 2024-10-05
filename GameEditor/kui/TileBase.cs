using Godot;
using KUtil;

namespace KUi
{
    public class TileBase
    {

        public TileBase(Chunk tileChunk)
        {
            TileChunk = tileChunk;
            Paper = new Color(0.0f, 0.0f, 0.0f, 1.0f);
			Ink = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        }

        protected Chunk TileChunk
        {
            get;
            private set;
        }

        protected Image	Image { get; set; }


		protected Color Ink
		{
			get;
			set;
		}

		protected Color Paper
		{
			get;
			set;
		}

    		protected void DrawByte(byte b, int x, int y, int zoom = 1, int gap=0)
		{
			for(int pixel = 0; pixel<8; pixel++)
			{
				Rect2I rect = new Rect2I(x+(pixel*zoom), y, zoom-gap ,zoom-gap);
				Image.FillRect(rect, 
					(b & 128) == 0 ? Paper : Ink);
				b <<= 1;
			}
		}
    }
}