
using ZX;
using ZX.Drawing;
using ZX.Game;
using ZX.Platform;
using ZX.Util;

namespace Pyjamarama.Inventory
{
    /// <summary>
    /// Draws the text of an object index.
    /// </summary>
    /// <remarks>
    /// The original data <see cref="IChunk"/>  had a string of object names, pointed to by a table of pointers,
    /// in the order of the object indices.
    /// Some objects can be empty or full, this is indicated by a flag stored in the 
    /// <see cref="IFlags"/> instance supplied by the platform.
    /// Colour of the text is supplied by the <see cref="Paper"/> and <see cref="Ink"/> properties, which
    /// are passed onto the character tile drawing.
    /// </remarks>
    internal class ObjectTextDrawer : IDrawer, IAttribute
    {
        #region Private Members;

        const byte CmdEndOfString = 0xFF;
        const byte CmdOffsetPosition = 0xFC;
        const byte CmdCheckAttribute = 0xFA;

        static readonly Chunk Full = new Chunk("full", 0x4000, 5, new byte[]{(byte)'F', (byte)'U', (byte)'L', (byte)'L', 0xFF });
        static readonly Chunk Empty = new Chunk("empty", 0x4000, 6, new byte[]{(byte)'E', (byte)'M', (byte)'P', (byte)'T', (byte)'Y', 0xFF });

        private readonly IDrawer _tileDrawer;
        private readonly IAttribute _attribute;
        private readonly IChunk _textTable;
        private readonly IChunk _text;

        private readonly IFlags _objectFlags;

        #endregion

        #region Construction

        public ObjectTextDrawer(
            IDrawer tileDrawer, 
            IChunk textTable,
            IChunk text,
            IFlags objectFlags)
        {
            _tileDrawer = tileDrawer;
            _attribute = _tileDrawer as IAttribute ?? throw new ArgumentException("tileDrawer should implement IAttribute.");
            _textTable = textTable;
            _text = text;
            _objectFlags =objectFlags;
            Paper = Palette.Black;
            Ink = Palette.Yellow;
        }

        #endregion

        #region IAttribute

        public Rgba Paper { get; set; }
        public Rgba Ink { get ; set; }

        #endregion

        #region IDrawer

        void IDrawer.Draw(ISurface surface, int index, int x, int y)
        {
            int next = CalculateStringAddress(index);
            _attribute.Ink = Ink;
            _attribute.Paper = Paper;

            FurnitureDrawLogic logic = new FurnitureDrawLogic()
            {
                Surface = surface,
                TileDrawer = _tileDrawer,
                Data = _text,
                X = x,
                Y = y,
                Index = next
            };

            Draw(logic);
        }

        private int CalculateStringAddress(int index)
        {
            return _textTable.Word(index*2) - _text.Start;
        }

        private void Draw(FurnitureDrawLogic logic)
        {
            while(logic.CurrentCode != CmdEndOfString)
            {
                switch (logic.CurrentCode)
                {
                    case CmdOffsetPosition:
                        logic.SetPositionCommand();
                        break;

                        default:
                        if ((logic.CurrentCode & 0x80) == 0)
                        {
                            logic.DrawTileAtCurrentPosition(logic.CurrentCode);
                        }
                        else
                        {
                            // Ignore anything above 0x7F
                            logic.Index++;
                            logic.X++;
                        }
                        break;
                }
            }

            logic.Index++; 

        
            if (logic.Index < logic.Data.Length && 
                logic.CurrentCode == CmdCheckAttribute)
            {
                CheckForEmptyOrFull(logic);
            }
        }

        private void CheckForEmptyOrFull(FurnitureDrawLogic logic)
        {
            IFlag flag = _objectFlags.GetByObjectIndex(logic.CurrentCode);

            if (flag != null)
            {
                IChunk chunk = Empty;

                if (flag.Value != 0)
                {
                    chunk = Full;
                }

                _attribute.Ink = Palette.BrightYellow;
                logic.X++;
                logic.Data = chunk;
                logic.Index = 0;
                Draw(logic);
            }
        }

        #endregion
    }
}