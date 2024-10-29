using System;
using System.Collections.Generic;
using Platform;
using KUtil;
using Range = KUtil.Range;

namespace KUi
{
    /// <summary>
    /// Abstract class for drawing tiled based objects.
    /// </summary>
    /// <remarks>
    /// Codes used by the draw string is a byte.
    /// All memory sizes are bytes.
    /// </remarks>
    public abstract class FurnitureDrawer : IFurnitureDrawer
    {
        #region Delegates
        protected delegate void CodeMethod(CodeArgs args);
        protected delegate void CursorMethod();
        #endregion

        #region Private Types
        protected class CodeArgs
        {
            public CodeArgs(byte[] args)
            {
                Args = args;
            }

            public byte[] Args { get; }
        }
    
        #endregion

        #region Fields
        /// <summary>
        /// Maps a byte range to a single code.
        /// </summary>
        protected IList<Range> Ranges { get; set; }

        /// <summary>
        /// Information on each code.
        /// </summary>
        protected Dictionary<byte, CodeInfo> CodeInfo { get; set; } 

        /// <summary>
        /// Collection of each method used for each code.
        /// </summary>
        protected Dictionary<byte, CodeMethod> CodeMethods { get; set; }

        private int _x;
        private int _y;
        #endregion

        #region Construction
        public FurnitureDrawer(Chunk tileStrings, 
            Chunk stringTable,
            IChunk tileChunk)
        {
            TileStringChunk = tileStrings;
            StringTableChunk = stringTable;
            TileDrawer = new TileDrawer(tileChunk);
            Zoom = 2;
        }
        #endregion

        #region Properties
        protected Chunk TileStringChunk { get; }
        private Chunk StringTableChunk { get; }        
        protected TileDrawer TileDrawer { get; }

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

        protected ISurface Image { get; set;}

        public int X 
        { 
            get => _x;
            protected set
            {
                _x = value;
                if(
                    Image != null &&
                    !DrawnOutOfBounds && 
                    !Image.IsInBounds(_x, _y))
                {
                    DrawnOutOfBounds = true;
                    OutOfBoundsHit = new System.Drawing.Point(_x, _y);
                }
            }
        }

        public int Y
        { 
            get => _y;
            protected set
            {
                _y = value;
                if(
                    Image != null &&
                    !DrawnOutOfBounds && 
                    !Image.IsInBounds(_x, _y))
                {
                    DrawnOutOfBounds = true;
                    OutOfBoundsHit = new System.Drawing.Point(_x, _y);
                }
            }
        }

        public int Zoom{ get; set; }

        /// <summary>
        /// Number of pixels that make up a screen character cell.
        /// </summary>
        public int CharSize => 8*Zoom;

        public bool DrawnOutOfBounds { get; protected set;}

        public System.Drawing.Point OutOfBoundsHit
        {
            get;
            set;
        }

        /// <summary>
        /// Keeps track of current position when decoding
        /// string.
        /// </summary>
        protected int Offset
        {
            get;
            set;
        }
        #endregion

        #region Draw Methods
        public void Draw(int x, int y, int index, ISurface image)
        {
            X = x;
            Y = y;
            Offset = CalculateAddressOffset(index);
            Image = image;

            DrawnOutOfBounds = false;
            TileDrawer.SetTileStart(0);

            do
            {
                DrawNext();
            } while(Offset != -1 && !DrawnOutOfBounds);

            Image = null;
        }

        private int CalculateAddressOffset(int index)
        {
            int word = StringTableChunk.Word(index*2);
            return  word - TileStringChunk.Start; 
        }

        private void DrawNext()
        {
            byte code = CheckForRangeCode(TileStringChunk[Offset++]);

            InvokeCodeMethod(code);                            
        }

        private byte CheckForRangeCode(byte code)
        {
            System.Diagnostics.Debug.Assert(Ranges != null, "Ranges should contain stuff");
            foreach(Range range in Ranges)
            {
                if(range.Within(code))
                {
                    code = range.LessThan;
                    break;
                }
            }

            return code;
        }

        private void InvokeCodeMethod(byte code)
        {
            CodeInfo info = CodeInfo[code];
            CodeArgs args = new CodeArgs(TileStringChunk.CopyRange(Offset-1, info.NumberOfArgs));
            CodeMethods[code].Invoke(args);         
            Offset+= info.NumberOfArgs-1;
        }
        #endregion


        #region Cursor Methods
        protected void CursorRight()
        {
            X += CharSize;
        }

        protected void CursorLeft()
        {
            X -= CharSize;
        }

        protected void CursorUp()
        {
            Y -= CharSize;
        }

        protected void CursorDown()
        {
            Y += CharSize;
        }
        #endregion
    }
}