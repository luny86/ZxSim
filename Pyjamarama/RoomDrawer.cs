
using ZX;
using ZX.Drawing;
using ZX.Platform;
using ZX.Util;
using ZX.Game;

namespace Pyjamarama
{
    /// <summary>
    /// Drawer for creating background images of each room.
    /// </summary>
    internal class RoomDrawer : IDrawer
    {
        #region Draw Commands

        private const byte CmdWalls = 0xf5;
        private const byte CmdEnd = 0xFF;
        private const byte CmdFlipOn = 0xF4;
        private const byte CmdFlipOff = 0xF3;
        private const byte CmdActionFlag = 0xDE;

        #endregion

        #region Private Members

        /// <summary>
        /// Data for all rooms.
        /// </summary>
        private readonly IChunk _data;

        /// <summary>
        /// A table of 16bit addresses pointing to each room.
        /// </summary>
        /// <remarks>
        /// The table will hold asbolute addresses, as if in the
        /// original memory. They can be used to calculate the offset
        /// of each room within the data chunk.
        private readonly IChunk _roomAddressTable;

        /// <summary>
        /// Drawer for handling the individual items that
        /// make up a room.
        /// </summary>
        private readonly IDrawer _furnitureDrawer;

        private readonly IDrawer _wallDrawer;

        private readonly IFlags _flags;
        #endregion

        #region Construction

        public RoomDrawer(
            IDrawer furnitureDrawer, 
            IDrawer wallDrawer, 
            IChunk data, 
            IChunk roomAddressTable,
            IFlags flags)
        {
            _furnitureDrawer = furnitureDrawer;
            _wallDrawer = wallDrawer;
            _data = data;
            _roomAddressTable = roomAddressTable;
            _flags = flags;
        }

        #endregion

        #region IDrawer

        void IDrawer.Draw(ISurface surface, int index, int x, int y)
        {
            bool endOfString = false;
            int offset = CalculateRoomIndex(index);

            surface.Fill(Palette.Black);

            while(!endOfString)
            {
                byte code = _data[offset];

                switch(code)
                {
                    case CmdWalls:
                    {
                        _wallDrawer.Draw(surface, 0,0,0);
                        offset++;
                    }
                        break;

                    case CmdEnd:
                        offset++;
                        endOfString = true;
                        break;

                    case CmdFlipOff:
                        // To do
                        offset++;
                        break;

                    case CmdFlipOn:
                        // To do
                        offset++;
                        break;

                    case CmdActionFlag:
                        {
                            int h = (_data[offset + 2] * 256) + _data[offset + 1];
                            string hex = string.Format("{0:X2}", h);

                            // Test flag
                            if (_flags[hex].Value == 0)
                            {
                                // Stop drawing.
                                endOfString = true;
                            }
                            else
                            {
                                // Z - Continue
                                offset += 3;
                            }
                        }
                        break;

                    default:
                        _furnitureDrawer.Draw(surface, _data[offset+2], _data[offset], _data[offset+1]);
                        offset+=3;
                        break;
                }

                
            }

        }

        #endregion

        #region Helpers

        private int CalculateRoomIndex(int index)
        {
            return _roomAddressTable.Word(index*2) - _data.Start;
        }

        #endregion
    }
}