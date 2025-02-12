
namespace ZX.Game
{
    /// <summary>
    /// Interface describing the flags
    /// object. Flags store miscellaneous
    /// values for the overall game.
    /// </summary>
    public interface IFlags
    {
        /// <summary>
        /// Adds a flag to the collection.
        /// </summary>
        /// <param name="name">Unique identifier.</param>
        /// <param name="initialValue">Value to set to.</param>
        void RegisterFlag(string name, IFlag initialValue);

        /// <summary>
        /// Gets a flag using its name.
        /// </summary>
        /// <param name="name">Name of flag.</param>
        IFlag this[string name]
        {
            get;
        }

        /// <summary>
        /// Gets a flag using related object index.
        /// </summary>
        /// <returns>The object index.</returns>
        IFlag GetByObjectIndex(int index);
    }
}