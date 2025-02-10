
namespace Pyjamarama
{
    /// <summary>
    /// Factory for creating Pyjamarama specific instances.
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Creates a furniture drawer by specifying which data to use for
        /// the tile-set and the furniture draw strings.
        /// </summary>
        FurnitureDrawer CreateFurnitureDrawer(string tileChunkName, string furnitureChunkName);
    }
}