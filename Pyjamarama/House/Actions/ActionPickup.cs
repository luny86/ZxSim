using System;
using System.Collections.Generic;

using Builder;
using Pyjamarama.Inventory;

namespace Pyjamarama.House
{
    internal class ActionPickUp : IAction, IUpdate, IBuildable
    {
        private IPlayer _player = null!;
        private IRoomProvider _roomProvider = null!;
        private IInventory _inventory = null!;

        public ActionPickUp()
        {

        }

        int IAction.DataSize
        {
            get
            {
                return 0;
            }
        }

        bool IAction.Invoke(IList<byte> data)
        {
            Console.WriteLine("Action pickup");
            IRoom room = _roomProvider.CurrentRoom;
            _player.JustPickedUp = true;

            int objectIndex = room.Slot.ObjectIndex;
            objectIndex = _inventory.RotatePockets(objectIndex);
            _roomProvider.UpdateSlot(objectIndex);

            // TODO - Sounds.

            return true;
        }

        bool IUpdate.Update()
        {
            // Return true, to show its finished.
            return true;
        }

        #region Buildable

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest(ClassNames.Wally, typeof(IPlayer));
            requests.AddRequest(ClassNames.RoomProvider, typeof(IRoomProvider));
            requests.AddRequest(ClassNames.Inventory, typeof(IInventory));
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _player = dependencies.TryGetInstance<IPlayer>(ClassNames.Wally);
            _roomProvider = dependencies.TryGetInstance<IRoomProvider>(ClassNames.RoomProvider);
            _inventory = dependencies.TryGetInstance<IInventory>(ClassNames.Inventory);
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

