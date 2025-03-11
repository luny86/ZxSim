

using System.Drawing;
using Builder;

namespace Pyjamarama.House
{
    /// <summary>
    /// Game test for checking if the player is in range of the
    /// object slot within the room.
    /// </summary>
    internal class TestPickupPosition : ITest, IBuildable
    {
        // Data indicies - Data held with logic commands.
        const int DataX = 0;
        const int DataY = 1;

        const int SlotMargin = 3;

        private IPlayer _player = null!;

        public TestPickupPosition()
        {
        }

        int ITest.TestDataSize
        {
            get { return 2; }
        }

        bool ITest.Test(IList<byte> data)
        {
            bool passed = false;

            if (!_player.IsDead && !_player.Disabled)
            {
                Point playerPosition = _player.Position;
                int pickupSlotX = (int)data[DataX];
                int pickupSlotY = (int)data[DataY];

                bool isInRange = 
                    (playerPosition.X >= (pickupSlotX - SlotMargin) && playerPosition.X < (pickupSlotX + SlotMargin)) &
                    (playerPosition.Y >= (pickupSlotY - SlotMargin) && playerPosition.Y < (pickupSlotY + SlotMargin));

                if (_player.JustPickedUp)
                {
                    if(!isInRange)
                    {
                        _player.JustPickedUp = false;
                    }
                }
                else
                {
                    if(isInRange)
                    {
                        Console.WriteLine("Pickup found...");
                        passed = true;
                    }
                }
            }

            return passed;
        }

        #region Buildable

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest(ClassNames.Wally, typeof(IPlayer));
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _player = dependencies.TryGetInstance<IPlayer>(ClassNames.Wally);
        }

        void IBuildable.EndBuild()
        {
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
        }

        #endregion
    }
}

