using System;
using KUtil;
using Platform;

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

        private readonly int _maxItems;
        private readonly DrawPositionCache _startCache;

        public FurnitureDraw(Chunk tileStrings, 
            Chunk stringTable,
            IChunk tileChunk,
            ISurface surface)
            {
                Drawer = new ThreeWeeks.FurnitureDrawer(
                    tileStrings,
                    stringTable,
                    tileChunk);
                Image = surface;

                Index = 9;  // First item
                Drawer.Zoom = 2;
    			Image.Create(512, 512);

                _maxItems = stringTable.Length/2;
                _startCache = new DrawPositionCache(
                    new Rectangle(0,0,512,512),
                    _maxItems,
                    Drawer.CharSize);
            }

        private FurnitureDrawer Drawer { get; }

        private ISurface Image { get; }

        public int Index 
        {
            get;
            set;
        }


        #region Public Interface        
        public void Draw()
        {
            Godot.GD.Print($"---- {Index} ----");
            System.Drawing.Point start = _startCache[Index];

            DrawReset();
            int timeout = 64;

            do
            {
                Godot.GD.Print("= "+start.ToString());
                Drawer.Draw(start.X, start.Y, Index, Image);     
                if(Drawer.DrawnOutOfBounds)
                {  
                    System.Drawing.Point p = Drawer.OutOfBoundsHit;
                    Godot.GD.Print(p.ToString());
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