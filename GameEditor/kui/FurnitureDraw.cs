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
    public class FurnitureDraw : TileBase, zx.IAttribute
    {
        private delegate void CursorMethod();

        /// <summary>
        /// Codes used by the draw decoder.
        /// </summary>
        enum Code : byte
        {
            DrawTile = 0x5c,    // Less then is a tile
            SetCursor = 0x9e,   
            DrawTileLoop = 0xc1,
            SetColourInk = 0xca,
            SetcolourInkBright = 0xd1,
            ToggleBrightness = 0xf1,
            DrawSolidTile = 0xf2,
            CursorLeftDown = 0xf3,
            SetColourMem = 0xf4,
            CursorRight = 0xf5,
            SwitchString = 0xf6,
            Draw2TilesLoopAcross = 0xf7,
            Draw2TilesLoopDown = 0xf8,
            DrawTileLoopStepUpLeft = 0xf9,
            DrawRectangle = 0xfa,
            SetAddress = 0xfb,
            TestFlag = 0xfc,
            SoftEnd = 0xfe,
            Exit = 0xFF
        }

        private readonly int _maxItems;
        private readonly DrawPositionCache _startCache;

        public FurnitureDraw(Chunk tileStrings, 
            Chunk stringTable,
            IChunk tileChunk,
            ISurface surface)
            : base(tileChunk, surface)
            {
                TileStringChunk = tileStrings;
                StringTableChunk = stringTable;
                Index = 9;  // First item
                Zoom = 2;
    			Image.Create(512, 512);
                _maxItems = stringTable.Length/2;
                _startCache = new DrawPositionCache(
                    new Rectangle(0,0,512,512),
                    _maxItems,
                    CharSize);
            }

        private Chunk TileStringChunk { get; }
        private Chunk StringTableChunk { get; }
        
        public int Index 
        {
            get;
            set;
        }

        /// <summary>
        /// Emulates the flags when drawing
        /// </summary>
        /// <remarks>
        /// Some items use a flag to determine which end part
        /// of the string to draw.
        /// </remarks>
        public bool Flag
        {
            get;
            set;
        }

        public int X { private get; set; }
        public int Y { private get; set;}

        /// <summary>
        /// Number of pixels that make up a screen character cell.
        /// </summary>
        private int CharSize => 8*Zoom;

        /// <summary>
        /// Offset value which points to the first tile
        /// in the memory, as set by the string decoder.
        /// </summary>
        /// <value></value>
        private int TileStart { get; set; }

        // Determines if Paper / Ink is bright
        private bool Bright { get; set; }

        #region Public Interface        
        public void Draw()
        {
            int offset = DrawReset();            
            do
            {
                offset = DrawNext(offset);
                if(!_startCache.IsInBounds(Index, X, Y))
                {
                    // start again....
                    offset = DrawReset();
                }
            } while(offset != -1);

            Image.EndDraw();
        }

        private int DrawReset()
        {
            System.Drawing.Point start = _startCache[Index];
            X = start.X;
            Y = start.Y;
            TileStart= 0;
            Ink = zx.Palette.BrightWhite;
            Paper = zx.Palette.Black;

            Image.BeginDraw();
			Image.Fill(new Rgba(0.5f, 0.5f, 0.5f, 1.0f));

            return CalculateAddressOffset(Index);
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

        private int CalculateAddressOffset(int index)
        {
            int word = StringTableChunk.Word(index*2);
            return  word - TileStringChunk.Start; 
        }

        private int CalculateTileAddr(int tileIndex)
        {
            return TileStart + (8 * tileIndex);
        }

        private int DrawNext(int offset)
        {
            byte code = TileStringChunk[offset++];

            if(code >= (byte)Code.SoftEnd)
            {
                return -1;
            }
            else if(code < (byte)Code.DrawTile)
            {
                int tile = CalculateTileAddr(code);

                DrawTile(X, Y, tile);
                CursorRight();
            }
            else if(code < (byte)Code.SetCursor)
            {
                // Some 8-bit maths so cursor goes in the right direction.
                byte byX = (byte)(code - (byte)0x7d);
                byte byY = TileStringChunk[offset++];
                sbyte offx = (sbyte)byX;
                sbyte offy = (sbyte)byY;

                X+=offx * CharSize;
                Y+=offy * CharSize;                
            }
            else if(code == (byte)Code.SetAddress)
            {
                ushort newAddr = TileStringChunk.Word(offset);
                TileStart = newAddr - TileChunk.Start;
                offset+=2;
            }
            else if(code < (byte)Code.DrawTileLoop)
            {
                int count = code - 0xa0;
                int tile = CalculateTileAddr(TileStringChunk[offset++]);

                for(int i=0;i<count;i++)
                {
                    DrawTile(X, Y, tile);
                    CursorDown();
                }
            }
            else if(code == (byte)Code.CursorLeftDown)
            {
                CursorLeft();
                CursorDown();
            }
            else if(code == (byte)Code.DrawSolidTile)
            {
                DrawTile(X, Y, 0x4335);
                CursorRight();
                return -1;
            }
            else if(code == (byte)Code.DrawTileLoopStepUpLeft)
            {
                int count = TileStringChunk[offset++];
                int tile = CalculateTileAddr(TileStringChunk[offset++]);

                for(int i=0; i<count; i++)
                {
                    DrawTile(X, Y, tile);
                    CursorUp();
                    CursorRight();
                }
            }
            else if(code == (byte)Code.CursorRight)
            {
                CursorRight();
            }
            else if(code == (byte)Code.Draw2TilesLoopDown)
            {
                offset = DrawTwoTilesLoopDown(offset);
            }
            else if(code == (byte)Code.ToggleBrightness)
            {
                zx.Palette.ToggleBright(this);
            }
            else if(code < (byte)Code.SetColourInk)
            {
                int c = code - 0xc2;
                zx.Palette.SetAttribute((byte)c, this);
            }
            else if(code < (byte)Code.SetcolourInkBright)
            {
                int c = code - 0x89;
                zx.Palette.SetAttribute((byte)c, this);
            }
            else if(code == (byte)Code.SwitchString)
            {
                offset = TileStringChunk.Word(offset) - TileStringChunk.Start;
            }
            else if(code == (byte)Code.SetColourMem)
            {
                byte c = TileStringChunk[offset++];
                zx.Palette.SetAttribute(c, this);
            }
            else if(code == (byte)Code.Draw2TilesLoopAcross)
            {
                offset = DrawTwoTilesLoopRight(offset);
            }
            else if(code == (byte)Code.DrawRectangle)
            {
                offset = DrawRectangle(offset);
            }
            else if(code == (byte)Code.TestFlag)
            {
                offset = TestFlag(offset);
                throw new NotImplementedException();
            }
            else
            { 
                offset = DrawLoopAndRight(offset);
            }

            return offset;
        }

        private int DrawTwoTilesLoopDown(int offset)
        {
            return DrawTwoTilesLoopAndMove(offset, CursorDown);
        }

        private int DrawTwoTilesLoopRight(int offset)
        {
            return DrawTwoTilesLoopAndMove(offset, CursorRight);
        }

        private int DrawTwoTilesLoopAndMove(int offset, CursorMethod method)
        {
            int amount = TileStringChunk[offset++];
            int tile1 = CalculateTileAddr(TileStringChunk[offset++]);
            int tile2 = CalculateTileAddr(TileStringChunk[offset++]);

            for(int i=0; i<amount; i++)
            {
                DrawTile(X, Y, tile1);
                method.Invoke();
                DrawTile(X, Y, tile2);
                method.Invoke();
            }
            return offset;
        }

        private int DrawRectangle(int offset)
        {
            int width = TileStringChunk[offset++];
            int height = TileStringChunk[offset++];
            int tile = CalculateTileAddr(TileStringChunk[offset++]);

            for(int y=0; y<height; y++)
            {
                for(int x=0; x<width;x++)
                {
                    DrawTile(
                        X+(x*CharSize),
                        Y+(y*CharSize), 
                        tile);
                }
            }

            return offset;
        }

        private int TestFlag(int offset)
        {
            offset++;   // Ignore flag index stored in string.
            if(Flag)
            {
                // Skip original ending for alternate ending.
                while(TileStringChunk[offset++] != (byte)Code.SoftEnd);
            }

            return offset;
        }

        private int DrawLoopAndRight(int offset)
        {
            int count = TileStringChunk[offset++];
            int tile = CalculateTileAddr(TileStringChunk[offset++]);
            for(int i=0; i< count; i++)
            {
                DrawTile(X, Y, tile);
                CursorRight();
            }

            return offset;
        }

        #region Cursor Methods
        private void CursorRight()
        {
            X += CharSize;
        }

        private void CursorLeft()
        {
            X -= CharSize;
        }

        private void CursorUp()
        {
            Y -= CharSize;
        }

        private void CursorDown()
        {
            Y += CharSize;
        }
        #endregion
    }
}