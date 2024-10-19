using KUtil;
using Platform;

namespace KUi
{
    public class RoomDraw : zx.IAttribute
    {
        private const byte CodeExit = 0xff;

        public RoomDraw(IReadOnlyChunk attrTable, 
            IReadOnlyChunk roomData,
            FurnitureDraw furnitureDraw,
            ISurface image)
        {
            AttrTable = attrTable;
            RoomData = roomData;
            FurnitureDraw = furnitureDraw;
            Image = image;
        }

        private IReadOnlyChunk AttrTable { get; }
        private IReadOnlyChunk RoomData { get; }
        private FurnitureDraw FurnitureDraw{ get; }
        private ISurface Image {get; }
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

        public int Index { get; set; }

        public void Draw()
        {
            zx.Palette.SetAttribute(AttrTable[Index], this);
            Image.Fill(Paper);

            int offset = StringSearch(Index);
            int next = 0;
            do
            {
                next = RoomData[offset++];
                if(next == CodeExit)
                {
                    break;
                }
                if(next == 0xfe)
                {
                    break; // For no
                }

                int y = RoomData[offset++];
                int i = RoomData[offset++];
                FurnitureDraw.X = next;
                FurnitureDraw.Y = y;
                FurnitureDraw.Index = i;
                // More flexible with image and what to draw... FurnitureDraw.Draw();
            }
            while(next != CodeExit);

        }

        private int StringSearch(int index)
        {
            int offset = 0;
            byte found = 0;

            do
            {
                while(found != CodeExit) 
                {
                    found = RoomData[offset++];
                }
            }
            while(--index > 0);

            return offset;
        }
    }
}