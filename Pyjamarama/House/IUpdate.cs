
namespace Pyjamarama.House
{
    /// <summary>
    /// Describes an update object for actions.
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// Called by the game loop until true
        /// is returned.
        /// </summary>
        bool Update();
    }
}

