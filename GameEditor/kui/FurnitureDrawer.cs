
using System.Collections.Generic;
using Platform;
using KUtil;

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
        protected delegate void CursorMethod();
        #endregion

        #region Fields
        /// <summary>
        /// Collection of each method used for each code.
        /// </summary>
        protected Dictionary<byte, CodeMethod> CodeMethods { get; set; }

        private int _x;
        private int _y;
        #endregion

        #region Construction
        public FurnitureDrawer(
            IChunk tileChunk,
            IDataContainer furniture)
        {
            Furniture = furniture;
            TileDrawer = new TileDrawer(tileChunk);
            Zoom = 2;
        }
        #endregion

        #region Properties
        protected TileDrawer TileDrawer { get; }
        private IDataContainer Furniture { get; }

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
        #endregion

        #region Draw Methods
        public void Draw(int x, int y, int index, ISurface image)
        {
            X = x;
            Y = y;
            Image = image;

            DrawnOutOfBounds = false;
            TileDrawer.SetTileStart(0);

            int debug = int.MaxValue; // Debug limit
            foreach(CodeArgs args in Furniture[index])
            {
                CodeMethods[args.Info.Code].Invoke(args);
                if(--debug == 0)
                {
                    break;
                }
            }

            Image = null;
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