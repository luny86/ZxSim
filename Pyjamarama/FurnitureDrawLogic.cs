using ZX;
using ZX.Util;
using ZX.Platform;
using ZX.Drawing;

namespace Pyjamarama
{
    /// <summary>
    /// Holds the fields and deals with the logic 
    /// of the furniture drawing.
    /// </summary>
    internal class FurnitureDrawLogic
    {
        #region Construction

        public FurnitureDrawLogic()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Surface to draw to.
        /// </summary>
        public ISurface Surface { get; init; } = null!;  

        /// <summary>
        /// Drawer for converting 8 bit data into bitmap tiles.
        /// </summary>
        public IDrawer TileDrawer { get; init; } = null!;

        /// <summary>
        /// Chunk holding the furniture draw strings.
        /// </summary>
        public IChunk Data { get; init; } = null!;

        public int X { get; set; }
        public int Y { get; set; }

        public int Index { get; set; }

        public int Offset { get; set; }

        /// <summary>
        /// Returns code value at current index position.
        /// </summary>
        public byte CurrentCode
        {
            get
            {
                return Data[Index];
            }
        }

        /// <summary>
        /// Gets the current data value as a tile index.
        /// </summary>
        public int CurrentAsTileIndex
        {
            get
            {
                return Data[Index] + Offset;
            }
        }
        #endregion

        #region Logic Methods

        private void DrawTileAndUpdatePosition(int index)
        {
            // Blit tile
            TileDrawer.Draw(Surface, index, X, Y);
            X++;
        }

        public void DrawTileAtCurrentPosition(int index)
        {
            DrawTileAndUpdatePosition(index);
            Index++;
        }

        public void SetPositionCommand()
        {
            // Offset position of next tile.
            X += Maths.Bit8_Signed(Data[Index + 1]);
            Y += Maths.Bit8_Signed(Data[Index + 2]);
            Index += 3;
        }

        public void SetOriginCommand(int startAddress)
        {
            // Offset start of tiles to use,
            // Allows original bytes to use more
            // then 256 tiles.
            Offset = (((256 * Data[Index + 2]) + Data[Index + 1]) - startAddress) / 8;

            Index += 3;
        }

        public void DrawRepeatedTileCommand()
        {
            // Draw the same item 'n' times in a line.
            for (int r = 0; r < Data[Index + 1]; r++)
            {
                DrawTileAndUpdatePosition(Offset+Data[Index+2]);
            }

            Index += 3;
        }

        public byte GetAttributeCommand()
        {
            byte attr = Data[Index+1];
            Index+=2;
            return attr;
        }
        #endregion
    }
}