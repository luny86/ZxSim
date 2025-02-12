
namespace ZX.Game
{
    /// <summary>
    /// Factory for creating ZX.Game items.
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Create a flag object.
        /// </summary>
        /// <param name="value">Intial value of flag.</param>
        /// <param name="relatedObjectIndex">Index of game object flag is related to.</param>
        /// <returns><see cref="IFlag"/>.</returns>
        IFlag CreateFlag(int value, int relatedObjectIndex);
    }
}