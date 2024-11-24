using KUtil;
using Platform;
using GameEditorLib.Platform;
using zx = GameEditorLib.zx;

namespace KUi
{
	public class RoomDraw : zx.IAttribute
	{
		private const byte CodeExit = 0xff;

		private readonly int _maxItems ;

		public RoomDraw(IReadOnlyChunk attrTable, 
			IReadOnlyChunk roomData,
			IFurnitureDrawer drawer,
			ISurface image)
		{
			AttrTable = attrTable;
			RoomData = roomData;
			Drawer = drawer;
			Image = image;

			_maxItems = 256;
		}

		private IReadOnlyChunk AttrTable { get; }
		private IReadOnlyChunk RoomData { get; }
		private IFurnitureDrawer Drawer { get; }
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
			Image.BeginDraw();
			zx.Palette.SetAttribute(AttrTable[Index], this);
			Image.Fill(Paper);

			int offset = StringSearch(Index);
			int size = Drawer.CharSize;
			int next;
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

				int x = next * size;
				int y = RoomData[offset++] * size;
				int i = RoomData[offset++];
				Drawer.Draw(x, y, i, Image);
			}
			while(next != CodeExit);

			Image.EndDraw();
		}

		private int StringSearch(int index)
		{
			if(index == 0)
			{
				return 0;
			}

			int offset = 0;

			do
			{
				while(RoomData[offset++] != CodeExit);
			}
			while(--index > 0);

			return offset;
		}

		#region Controls
		public void NextItem()
		{
			if(++Index >= _maxItems)
			{
				Index = 0;
			}

			Draw();
		}

		public void PreviousItem()
		{
			if(--Index < 0)
			{
				Index = _maxItems - 1;
			}

			Draw();
		}
		#endregion

	}
}
