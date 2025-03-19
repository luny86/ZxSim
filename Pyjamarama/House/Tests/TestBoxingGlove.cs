

using System.Drawing;
using Builder;
using Pyjamarama.Inventory;
using ZX.Drawing;
using Bindings;
using ZXD = ZX.Drawing;

namespace Pyjamarama.House
{
    /// <summary>
    /// BOxing glove test.
    /// </summary>
    /// <remarks>
    /// The boxing glove is a trap that is triggered when the player
    /// exits a right hand door, in the room that the trap is set.
    /// Each time the trap is set:
    /// The boxing glove will pop out,
    /// punch Wally who will fall down and lose energy
    /// The trap will then be set in the next room from a list
    /// of rooms that can hold the trap.
    /// </remarks>
    internal class TestBoxingGlove: ITest, IUpdate, IBuildable
    {
        #region Metrics

        /// <summary>
        /// current state of the Update action.
        /// </summary>
        /// <value>The state.</value>
        enum State
        {
            Punch,
            Sit,
            Wait1,
            Wait2,
            End
        }

        const int TowelObjectIndex = 0x15;
        const int BoxingGloveStartFrame = 85;

        // List of rooms trap can be set, in order.
        static int[] rooms = new int[] { 0x11, 0x0f, 0x03, 0x07, 0x01, 0x14, 0x0a };

        #endregion

        #region Dependencies

        private IInventory  _inventory = null!;
        private IPlayer _player = null!;
        private IRoomProvider _room = null!;
        private ZXD.IFactory _drawFactory = null!;
        private ZXD.IScreen _screen = null!;
        private IBindingManager _bindings = null!;

        #endregion

        #region Members

        /// <summary>
        /// Used to keep a copy of the bound value 'RoomIndex'
        /// so we know which room the camera is currently on.
        /// </summary>
        private int _roomIndex;
        private int _tripAnimationCount;

        #endregion

        #region Construction

        public TestBoxingGlove()
        {
        }

        #endregion

        #region Properties

        int ITest.TestDataSize => 0;


        private int RoomTrapIsSetIn
        {
            get;
            set;
        }

        private State UpdateState
        {
            get;
            set;
        }

        #endregion 

        #region ITest

        bool ITest.Test(IList<byte> data)
        {
            bool trip = true;

            if (!_inventory.IsCarrying(TowelObjectIndex) &&
                _roomIndex == rooms[RoomTrapIsSetIn]
                )
            {
                // Not carrying towel and in same room as glove
                // triggers the trap.
                trip = false;
                SetTrapInNextRoom();

                UpdateState = State.Punch;
                _player.Disabled = true;

                IAnimation animation = _drawFactory.CreateStaticAnimation(
                    "glove",
                    BoxingGloveStartFrame, BoxingGloveStartFrame+7,
                    2);
                
                animation.Hold = 14;
                animation.Position = new Point(0xe8, 0x98);
                animation.AnimationComplete += AnimationComplete;

                if(_screen["Animation"] is IAnimationLayer layer)
                {
                    layer.RegisterAnimation(animation);
                }
            }

            // As this is an IUpdate, returning
            // false will call that failed action.
            return trip;

        }
       
        private void SetTrapInNextRoom()
        {
            RoomTrapIsSetIn++;
            if(RoomTrapIsSetIn >= rooms.Length)
            {
                RoomTrapIsSetIn = 0;
            }
        }

        #endregion

        #region IUpdate

        private void AnimationComplete(object? sender, EventArgs e)
        {
            // Invoked once the glove animation (Punch State)
            // has finished animationg.
            UpdateState = State.Sit;
        }

        bool IUpdate.Update()
        {
            switch(UpdateState)
            {
                case State.Punch:
                    _player.Position = new Point(_player.Position.X - 1, _player.Position.Y);
                    // Animation callback will set next state.
                    break;

                case State.Sit:
                    SittingWaitState();
                    break;

                case State.Wait1:
                    SittingWaitState();
                    break;

                case State.Wait2:
                    FinalWaitState();
                    break;
            }

            return UpdateState == State.End;
        }

        private void SitState()
        {
            int fy = _room.CurrentRoom.FloorHeight;

            _player.Falling();

            Point p = _player.Position;
            p.Y += 3;
            _player.Position = p;

            if (p.Y >= fy)
            {
                p.Y = fy;
                _player.Position = p;
                _player.Frame = 0x9B;
                _tripAnimationCount = 0x28;
                UpdateState = State.Wait1;
            }
        }

        private void SittingWaitState()
        {
            if (_tripAnimationCount < 0x14)
            {
                _player.Frame = 0x9d;
            }                

            if (--_tripAnimationCount == 0)
            {
                _player.Frame = 0x10;
                UpdateState = State.Wait2;
                _tripAnimationCount = 0x28;
                _inventory.LoseEnergy(5);
            }
        }

        private void FinalWaitState()
        {
            if(--_tripAnimationCount == 0)
            {
                _player.Disabled = false;
                UpdateState = State.End;
            }
        }

        #endregion
        
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
            requests.AddRequest(ClassNames.Inventory, typeof(IInventory));
            requests.AddRequest(ClassNames.Wally, typeof(IPlayer));
            requests.AddRequest(ZXD.ClassNames.Factory, typeof(ZXD.IFactory));
            requests.AddRequest(ZXD.ClassNames.Screen, typeof(ZXD.IScreen));
            requests.AddRequest(ClassNames.RoomProvider, typeof(IRoomProvider));
            requests.AddRequest(Bindings.ClassNames.BindingManager, typeof(IBindingManager));
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _inventory = dependencies.TryGetInstance<IInventory>(ClassNames.Inventory);
            _player = dependencies.TryGetInstance<IPlayer>(ClassNames.Wally);
            _drawFactory = dependencies.TryGetInstance<ZXD.IFactory>(ZXD.ClassNames.Factory);
            _screen = dependencies.TryGetInstance<ZXD.IScreen>(ZXD.ClassNames.Screen);
            _room = dependencies.TryGetInstance<IRoomProvider>(ClassNames.RoomProvider);
            _bindings = dependencies.TryGetInstance<IBindingManager>(Bindings.ClassNames.BindingManager);
        }

        void IBuildable.EndBuild()
        {
            _bindings.Bind(BoundValueNames.RoomIndex, RoomIndex_Change);
        }

        /// <summary>
        /// Invoked everytime the displayed room changes.
        /// </summary>
        private void RoomIndex_Change(string name, Type type, object? value)
        {
            if(name == BoundValueNames.RoomIndex &&
                value is int index)
            {
                _roomIndex = index;
            }
        }

        #endregion
    }
}