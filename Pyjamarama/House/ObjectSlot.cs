

namespace Pyjamarama.House
{
    /// <summary>
    /// Single slot, for a room,
    /// which holds an object.
    /// </summary>
    public class ObjectSlot : IObjectSlot
    {
        // Start of objects in the sprite table.
        const int ObjectIndexOffset = 0x50;
        // Offset of slot on screen.
        const int SlotYOffset = 0x10;

        // Create an empty slot.
        public ObjectSlot(int x, int y, int index)
        {
            X = x;
            Y = y;
            InitialIndex = index;
            Enabled = true;
        }

        public void NewGame()
        {
            ObjectIndex = InitialIndex;
        }

        public void NewLevel()
        {}

        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// X position of object on screen.
        /// </summary>
        /// <value>The x.</value>
        public int X
        {
            get;
            set;
        }

        /// <summary>
        /// Y position of object on screen.
        /// </summary>
        /// <value>The y.</value>
        public int Y
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the index of the object
        /// that is currently in the slot.
        /// </summary>
        /// <value>The index of the object.</value>
        public int ObjectIndex
        {
            get;
            set;
        }

        // Fixed value for new games.
        private int InitialIndex
        {
            get;
            set;
        }
    }
}

