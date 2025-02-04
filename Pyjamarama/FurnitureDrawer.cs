
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
        #endregion

        #region Construction
        public FurnitureDrawer(IDrawer tileDrawer, IChunk data)
        {
            _tileDrawer = tileDrawer ?? throw new ArgumentNullException(nameof(tileDrawer));
            _data = data ?? throw new ArgumentNullException(nameof(data));
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

        // Drawing attributes
        private int X { get; set; }
        private int Y { get; set; }

        #endregion

        #region IDrawer
        void IDrawer.Draw(ISurface surface, int index, int x, int y)
        {
            IAttribute attribute = _tileDrawer as IAttribute 
                ?? throw new InvalidCastException("Tile drawer should implement IAttribute.");

            int offset = 0;
            int i = Table[index];
            bool done = false;

            X = x;
            Y = y;

            while(!done)
            {
                byte code = _data[i];

                if( (code & CmdFlag) == 0)
                {
                    // Blit tile
                    _tileDrawer.Draw(surface, offset+code, X, Y);
                    X++;
                    i++;
                }
                else
                {
                    switch(code)
                    {
                        case CmdEnd:
                            i++;
                            done = true;
                            break;

                        case CmdColor:
                            Palette.SetAttribute(_data[i+1], attribute);
                            i+=2;
                            break;

                        case CmdPosition:
                            {
                                // Offset position of next tile.
                                X += Maths.Bit8_Signed(_data[i + 1]);
                                Y += Maths.Bit8_Signed(_data[i + 2]);
                                i += 3;
                            }
                            break;

                        case CmdOrigin:
                            {
                                // Offset start of tiles to use,
                                // Allows original bytes to use more
                                // then 256 tiles.
                                int h = (256 * _data[i + 2]) + _data[i + 1];
                                
                                if (h >= OriginStartAddr)
                                {
                                    offset = (h - OriginStartAddr) / 8;
                                }

                                i += 3;
                            }
                            break;

                        case CmdRepeat:
                        default:                            
                        {
                                // Draw the same item 'n' times in a line.
                                for (int r = 0; r < _data[i + 1]; r++)
                                {
                                    _tileDrawer.Draw(surface, offset+_data[i+2], X, Y);
                                    X++;
                                }

                                i += 3;
                            }
                            break;
                    }
                }
            }
        }
        #endregion

    }
}