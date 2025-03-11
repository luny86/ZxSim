namespace Pyjamarama.House
{
    /// <summary>
    /// Class for handling and displaying a room
    /// in Pyjamarama.
    /// </summary>
    public class Room : IRoom
    {
        /// <summary>
        /// Creates an instance of a <see cref="Room"/> 
        /// </summary>
        public Room()
        {
        }

        /// <summary>
        /// Reset data for a new game.
        /// </summary>
        public void NewGame()
        {
            (Slot as IObjectSlot).NewGame();
            Visited = false;
        }

        /// <summary>
        /// Position and type of object in room.
        /// </summary>
        /// <value>The slot.</value>
        public IObjectSlot Slot
        {
            get;
            init;
        } = null!;

        /// <summary>
        /// Determines if the rooms has been visited.
        /// PArt ofthe game finished percentage calculation.
        /// </summary>
        /// <value><c>true</c> if visited; otherwise, <c>false</c>.</value>
        public bool Visited
        {
            get;
            set;
        }

        public int FloorHeight
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"Room - Slot {Slot}";
        }
    }
}
