using KUtil;
using GameEditorLib.Platform;
using GameEditorLib.Ui;

namespace KUi
{
	/// <summary>
	/// Decodes the furniture strings into a tiled graphic on screen.
	/// </summary>
	/// <remarks>
	/// There are three memory chunks, address table, tiles and furniture.
	/// The table holds the start of each furniture string.
	/// The furniture string uses draw codes to put the tiles together.
	/// The tiles hold the bitmaps of each tile.
	/// </remarks>
	public class FurnitureDraw
	{

		private int _maxItems;
		private DrawPositionCache _startCache;

		public FurnitureDraw()
		{
		}
		
		public void Initialise( 
			ISurface surface,
			IFurnitureDrawer drawer,
			int maxItems)
			{
				Drawer = drawer;
				Image = surface;

				Index = 9;  // First item
				Image.Create(512, 512);

				_maxItems = maxItems;
				_startCache = new DrawPositionCache(
					new Rectangle(0,0,512,512),
					_maxItems,
					Drawer.CharSize);
			}

		private IFurnitureDrawer Drawer { get; set; }

		private ISurface Image { get; set; }

		public int Index 
		{
			get;
			set;
		}


		#region Public Interface        
		public void Draw()
		{
			System.Drawing.Point start = _startCache[Index];

			DrawReset();
			int timeout = 64;

			do
			{
				Drawer.Draw(start.X, start.Y, Index, Image);     
				if(Drawer is IBoundsDraw bounds &&
					bounds.DrawnOutOfBounds)
				{  
					System.Drawing.Point p = bounds.OutOfBoundsHit;
					_startCache.IsInBounds(Index, p.X, p.Y);
					start = _startCache[Index];
					// start again....
					DrawReset();
				}
				else
				{
					break;
				}
			} while(--timeout > 0);

			Image.EndDraw();
		}

		private void DrawReset()
		{
			Image.BeginDraw();
			Image.Fill(new Rgba(0.5f, 0.5f, 0.5f, 1.0f));
		}

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
