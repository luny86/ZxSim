
using ZX;
using ZX.Util;
using ZX.Drawing;
using ZX.Platform;

namespace Pyjamarama
{
    public class FurnitureDrawer : IDrawer
    {
        #region Code Commands
        // TODO - Get offset from tile chink instead.
        private const int OriginStartAddr = 0xC1A0;
        private const int CmdEnd = 0xFF;
        private const int CmdColor = 0xFE;
        private const int CmdPosition = 0xFC;
        private const int CmdOrigin = 0xFB;
        private const int CmdRepeat = 0xFA;

        private const byte CmdFlag = 0x80;       // Mask for command flag.
        #endregion

        #region Private Members

        static int[] Table = new int[]
        {
            // Indicies to start of each item within the furniture chunk.
            0x00,0x353,0xbf,0x499,0xbf1,0x54d,0x939,0xcfa,0xdea,0xe76,0xbc4,0xd61,0xd92,0x2da,0xe19,0x300,0xdc3,0x9e5,0xa91,0x4cf,0x4dc,0x9c0,0x583,0x5b2,0x5d8,0xa30,0xa62,
            0x46f,0x64b,0x621,0x636,0x6ed,0x6b7,0x79f,0x326,0x3c6,0xad6,0x514,0x4e9,0x2ab,0x290,0x543,0xb48,0x802,0x868,0xd27,0x239,0x19f,0x729,0xb06,0x99b,0x47,0x897,0x1cc,
            0xb6f,0xfd,0x111,0x215,0x8f0,0x65,0x72,0x455,0x90,0x966,0x92a,0x165
        };

        private readonly IDrawer _tileDrawer;
        private readonly IChunk _data;
        private readonly IAttributeTable _attributeTable;
        #endregion

        #region Construction
        public FurnitureDrawer(IDrawer tileDrawer, IChunk data, IAttributeTable attributeTable)
        {
            _tileDrawer = tileDrawer ?? throw new ArgumentNullException(nameof(tileDrawer));
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _attributeTable = attributeTable;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the image drawn is flipped.
        /// </summary>
        /// <value><c>true</c> if flip; otherwise, <c>false</c>.</value>
        static public bool Flip
        {
            get;
            set;
        }

        public bool DrawAsDouble
        {
            get;
            set;
        }
        #endregion

        #region IDrawer
        void IDrawer.Draw(ISurface surface, int itemIndex, int x, int y)
        {
            IAttribute attribute = _tileDrawer as IAttribute 
                ?? throw new InvalidCastException("Tile drawer should implement IAttribute.");

            FurnitureDrawLogic logic = new FurnitureDrawLogic()
            {
                Surface = surface,
                TileDrawer = _tileDrawer,
                Data = _data,
                X = x,
                Y = y,
                Index = Table[itemIndex]
            };

            bool done = false;

            while(!done)
            {
                byte code = logic.CurrentCode;

                if(code < CmdFlag)
                {
                    logic.DrawTileAtCurrentPosition(logic.CurrentAsTileIndex);
                }
                else
                {
                    switch(code)
                    {
                        case CmdEnd:
                            logic.Index++;
                            done = true;
                            break;

                        case CmdColor:
                        {
                            byte colour = logic.GetAttributeCommand();
                            Palette.SetAttribute(colour, attribute);
                            _attributeTable.SetAt(logic.X, logic.Y, colour);
                        }
                            break;

                        case CmdPosition:
                            logic.SetPositionCommand();
                            break;

                        case CmdOrigin:
                            logic.SetOriginCommand(OriginStartAddr);
                            break;

                        case CmdRepeat:
                        default:
                            logic.DrawRepeatedTileCommand(); 
                            break;
                    }
                }
            }
        }

        #endregion

    }
}