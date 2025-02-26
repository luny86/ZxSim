
using ZX;
using ZX.Drawing;


namespace Pyjamarama
{
    /// <summary>
    /// Factory for creating Pyjamarama specific instances.
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Creates a furniture drawer which can decode Pyjamarama
        /// furniture strings.
        /// </summary>
        /// <param name="tileChunkName">Name of memory chunk holding tile bitmaps.</param>
        /// <param name="furnitureChunkName">Name of memory chunk holding furniture strings.</param>
        /// <returns></returns>
        IDrawer CreateFurnitureDrawer(string tileChunkName, string furnitureChunkName);

        /// <summary>
        /// Creates a room drawer by specifying which data to use for
        /// the tile-set and the furniture draw strings.
        /// </summary>
        IDrawer CreateRoomDrawer(string addressTableName, string dataChunkName, string tileChunkName, string furnitureChunkName);

        /// <summary>
        /// Create an attribute table to emulate the ZX Attribute display
        /// </summary>
        /// <returns><see cref="IAttributeTable"/> instance.</returns>
        IAttributeTable GetAttributeTable();
    }
}