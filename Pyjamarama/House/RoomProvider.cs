
using Bindings;
using Builder;
using ZX;
using ZX.Drawing;
using ZX.Game;
using ZX.Platform;
using ZX.Util;

namespace Pyjamarama.House
{
    /// <summary>
    /// Holds a list of all the available rooms.
    /// </summary>
    internal class RoomProvider : IBuildable, IAttribute, IGameStatic
    {
        #region Members

        // Describing commands used when drawing the room.
        private const byte CmdWalls = 0xf5;
        private const byte CmdEnd = 0xFF;
        private const byte CmdFlipOn = 0xF4;
        private const byte CmdFlipOff = 0xF3;
        private const byte CmdActionFlag = 0xDE;

        //
        // Dependencies

        private IFactory _factory = null!;
        ZX.Platform.IFactory _platformFactory = null!;
        IScreen _screen = null!;
        private IBindingManager _bindingManager = null!;

        private BackgroundLayer _layer = null!;
        #endregion

        /// <summary>
        /// Creates an instance of a <see cref="RoomList"/> 
        /// </summary>
        public RoomProvider()
        {
            Rooms = new List<Room>();
            //RoomEvents = new RoomEvents();
            Palette.SetAttribute(0, this);
        }

        #region Properties

        /// <summary>
        /// Gets the attributes from the screen
        /// as a table.
        /// </summary>
        /// <value>The attributes table.</value>
        public IAttributeTable Attributes
        {
            get;
            private set;
        } = null!;

        public Rgba Paper { get; set; } = null!;
        public Rgba Ink { get; set; } = null!;

		/// <summary>
		/// Gets the number of rooms that exist.
		/// </summary>
		public int Count
		{
			get
			{
				return this.Rooms.Count;
			}
		}

        /// <summary>
        /// Get / sets the list of rooms.
        /// </summary>
        /// <value>The rooms.</value>
        private List<Room> Rooms
        {
            get;
            set;
        }


        /// <summary>
        /// Gets a room based on index.
        /// </summary>
        /// <param name="index">Index of room required.</param>
        public Room this[int index]
        {
            get
            {
                if (index < Rooms.Count)
                {
                    return Rooms[index];
                }

                throw new IndexOutOfRangeException("index");
            }
        }

        #endregion 

        void IGameStatic.NewGame()
        {
            foreach (Room room in Rooms)
            {
                room.NewGame();
            }
        }

        /// <summary>
        /// Call when Wally enters a new room.
        /// </summary>
        /// <param name="game">Game.</param>
        void IGameStatic.NewLevel()
        {
        }
            
        /// <summary>
        /// Creates the original games' data.
        /// </summary>
        public void Initialise()
        {
            Room[] data = new Room[]
            {
                // Room 00
                new Room() { FloorHeight = 0x88, Slot = new ObjectSlot(0x3a, 0x88,2)},
                // Room 01
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x49, 0x98,3)},
                // Room 02
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x98, 0x98,4)},
                // Room 03
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0xcc, 0x70,5)},
                // Room 04
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x78, 0x98,6)},
                // Room 05
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x68, 0x98,7)},
                // Room 06
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x78, 0x97,8)},
                // Room 07
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x60, 0x98,9)},
                // Room 08
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x08, 0x80,10)},
                // Room 09
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0xc8, 0x28,11)},
                // Room 0a
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x86, 0x68,12)},
                // Room 0b
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x48, 0x98,13)},
                // Room 0c
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0xb0, 0x98,14)},
                // Room 0d
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0xc0, 0x78,15)},
                // Room 0e
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x8c, 0x68,16)},
                // Room 0f
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x90, 0x70,17)},
                // Room 10
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x32, 0x80,18)},
                // Room 11
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x74, 0x70,19)},
                // Room 12
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0xc0, 0x80,20)},
                // Room 13
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x78, 0x98,21)},
                // Room 14
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x38, 0x98,22)},
                // Room 15
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x60, 0x98,23)},
                // Room 16
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0xde, 0x98,24)},
                // Room 17
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x78, 0x68,25)},
                // Room 18
                new Room() { FloorHeight = 0x68, Slot = new ObjectSlot(0x64, 0x68,26)},
                // Room 19
                new Room() { FloorHeight = 0x88, Slot = new ObjectSlot(0x46, 0x88,27)},
                // Room 1a
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0x68, 0x70,28)},
                // Room 1b
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0xb0, 0x70,29)},
                // Room 1c
                new Room() { FloorHeight = 0x88, Slot = new ObjectSlot(0x32, 0x88,30)},
                // Room 1d
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0xa8, 0x60,31)},
                // Room 1e
                new Room() { FloorHeight = 0x98, Slot = new ObjectSlot(0,0, 0) { Enabled = false} }
            };

            foreach (Room room in data)
            {
                // Set to original start object.
                room.NewGame();
                Rooms.Add(room);
            }
        }

        private void RoomIndexValueChanged(string name,  Type  type, object? value)
        {
            _layer.Update();
        }

        #region IBuildable

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
        }

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest(ClassNames.Factory, typeof(IFactory));
            requests.AddRequest(ZX.Platform.ClassNames.Factory, typeof(ZX.Platform.IFactory));
            requests.AddRequest(ZX.Drawing.ClassNames.Screen, typeof(IScreen));
            requests.AddRequest(Bindings.ClassNames.BindingManager, typeof(IBindingManager));
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _factory = dependencies.TryGetInstance<IFactory>(ClassNames.Factory);
            _platformFactory = dependencies.TryGetInstance<ZX.Platform.IFactory>(ZX.Platform.ClassNames.Factory);
            _screen = dependencies.TryGetInstance<IScreen>(ZX.Drawing.ClassNames.Screen);
            _bindingManager = dependencies.TryGetInstance<IBindingManager>(Bindings.ClassNames.BindingManager);
        }

        void IBuildable.EndBuild()
        {
            Attributes = _factory.GetAttributeTable();
            CreateBackgroundLayer();
            _bindingManager.CreateObject<int>(BoundValueNames.RoomIndex, 1);
            _bindingManager.Bind(BoundValueNames.RoomIndex, RoomIndexValueChanged);
        }

    
        private void CreateBackgroundLayer()
        {
            IDrawer drawer = _factory.CreateRoomDrawer("RoomPointers", "Rooms", "Tiles", "Furniture");
            ISurface bg = _platformFactory.CreateSurface();

            _layer = new BackgroundLayer(drawer, "background", bg, (int)LayerZOrders.Background)
            {
                RoomIndex = 1
            };

            // TEST
            _layer.Update();
            //

            _screen.AddLayer(_layer);	
        }
        #endregion
    }
}

