
using System.Drawing;
using System.Text;
using ZX.Util;

namespace ZX
{
    /// <summary>
    /// Simulates the attribute table
    /// of a ZX Spectrum.
    /// </summary>
    public class AttributeTable : IAttributeTable
    {
        /// <summary>
        /// Create a new instance of <see cref="AttributeTable"/> 
        /// </summary>
        public AttributeTable()
        {
            Table = new byte[ZX.Hardware.ScreenWidthInColumns * ZX.Hardware.ScreenHeightInColumns];
        }

        /// <summary>
        /// Gets or sets the table of attribute values.
        /// </summary>
        private byte[] Table
        {
            get;
            set;
        }

        /// <summary>
        /// Clear the table using a given colour pair.
        /// </summary>
        /// <param name="attribute">Raw byte colour to fill with.</param>
        public void Clear(byte attribute)
        {
            for (int i=0; i<Table.Length; i++)
            {
                Table[i] = attribute;
            }
        }

        /// <summary>
        /// Sets attribute colour at given coords.
        /// </summary>
        /// <param name="position">The (x,y) coordinates.</param>
        /// <param name="colours">Colours to set.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If 'x' or 'y' are out of range based on the
        /// ZX Spectrum screen dimensions.
        /// </exception>
        public void SetAt(Point position, byte colours)
        {
            SetAt(position.X, position.Y, colours);
        }

        /// <summary>
        /// Sets attribute colour at given coords.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="colours">Colour to set.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If 'x' or 'y' are out of range based on the
        /// ZX Spectrum screen dimensions.
        /// </exception>
        public void SetAt(int x, int y, byte colours)
        {
            if (x < 0 || x >= ZX.Hardware.ScreenWidthInColumns)
            {
                throw new ArgumentOutOfRangeException("position.X");
            }

            if (y < 0 || y >= ZX.Hardware.ScreenHeightInColumns)
            {
                throw new ArgumentOutOfRangeException("position.Y");
            }

            Table[x + (y * ZX.Hardware.ScreenWidthInColumns)] = colours;
        }

        /// <summary>
        /// Get attribute value at x and y.
        /// </summary>
        /// <returns>The <see cref="ZXSpectrum.IColourPair"/>.</returns>
        /// <param name="position">The (x,y) coordinates.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If 'x' or 'y' are out of range based on the
        /// ZX Spectrum screen dimensions.
        /// </exception>
        public byte GetAt(Point position)
        {
            if (position.X < 0 || position.X >= ZX.Hardware.ScreenWidthInColumns)
            {
                throw new ArgumentOutOfRangeException("position.X");
            }

            if (position.Y < 0 || position.Y >= ZX.Hardware.ScreenHeightInColumns)
            {
                throw new ArgumentOutOfRangeException("position.Y");
            }

            return Table[position.X + (position.Y * ZX.Hardware.ScreenWidthInColumns)];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Attributes:");
            for(int y =0; y <ZX.Hardware.ScreenHeightInColumns; y++)
            {
                for(int x=0; x< ZX.Hardware.ScreenWidthInColumns; x++)
                {
                    sb.Append($"{Table[x+(y * ZX.Hardware.ScreenWidthInColumns)]:x2}, ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}

