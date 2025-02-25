
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
        private readonly IChunk _tiles;
        private readonly IChunk _textTable;
        private readonly IChunk _text;

        private readonly IFlags _objectFlags;

        #endregion

        #region Construction

        public ObjectTextDrawer(
            IDrawer tileDrawer, 
            IChunk tiles,
            IChunk textTable,
            IChunk text,
            IFlags objectFlags)
        {
            _tileDrawer = tileDrawer;
            _attribute = _tileDrawer as IAttribute ?? throw new ArgumentException("tileDrawer should implement IAttribute.");
            _tiles = tiles;
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
            Draw(surface, index, _text, next, x, y);
        }

        private int CalculateStringAddress(int index)
        {
            return _textTable.Word(index*2) - _text.Start;
        }

        private void Draw(ISurface surface, int index, IChunk text, int start, int x, int y)
        {
            int next =start;

            while(text[next] != CmdEndOfString)
            {
                switch (text[next])
                {
                    case CmdOffsetPosition:
                        // Offset position
                        x += ZX.Maths.Bit8_Signed(text[next + 1]);
                        y += ZX.Maths.Bit8_Signed(text[next + 2]);
                        next += 3;
                        break;


                        default:
                        if ((text[next] & 0x80) == 0)
                        {
                            _tileDrawer.Draw(
                                surface,
                                text[next],
                                x,
                                y);
                        }
                        next++;
                        x++;
                        // Ignore anything above 0x7F
                        break;
                }
            }

            next++; 

        
            if (next < text.Length && text[next] == CmdCheckAttribute)
            {
                CheckForEmptyOrFull(surface, index, x, y);
            }
        }

        private void CheckForEmptyOrFull(ISurface surface, int index, int x, int y)
        {
            IFlag flag = _objectFlags.GetByObjectIndex(index);

            if (flag != null)
            {
                IChunk chunk = Empty;

                if (flag.Value != 0)
                {
                    chunk = Full;
                }

                _attribute.Ink = Palette.BrightYellow;
                Draw(surface, index, chunk, 0, x + 1, y);
            }
        }

        #endregion
    }
}