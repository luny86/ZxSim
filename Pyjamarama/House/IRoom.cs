
namespace Pyjamarama.House
{
    /// <summary>
    /// Describes a room.
    /// </summary>
    public interface IRoom
    {
        /// <summary>
        /// Gets a reference to the object slot
        /// for interactive object in each room.
        /// </summary>
        IObjectSlot Slot { get; }
    }
}