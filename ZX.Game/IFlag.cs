
namespace ZX.Game
{
    /// <summary>
    /// Interface for a flag.
    /// Flags store values for game objects.
    /// A flag can have a relationship with an object using
    /// and index.
    /// </summary>
    public interface IFlag
    {
        /// <summary>
        /// Gets value of flag.
        /// </summary>
        int Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the associated object, if one.
        /// </summary>
        int Object
        {
            get;
        }
    }
}