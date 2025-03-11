
namespace Pyjamarama.House
{
    /// <summary>
    /// Describes the room provider for handling room details.
    /// </summary>
    public interface IRoomProvider
    {
        IRoom CurrentRoom { get; }

        void SetRoom(int newRoom);
        
        void UpdateSlot(int newObjectIndex);
    }
}