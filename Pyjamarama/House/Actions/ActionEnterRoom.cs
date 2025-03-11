

using System.Drawing;
using Builder;
using ZX.Platform;

namespace Pyjamarama.House
{
    public class ActionEnterRoom : IAction, IUpdate, IBuildable
    {
        const int OffsetY = 40;

        private IPlayer _player = null!;
        private IRoomProvider _roomProvider = null!;

        private int newRoom;
        private Point newPos = Point.Empty;
        private int count;

        public ActionEnterRoom()
        {
        }

        int IAction.DataSize
        {
            get
            {
                return 3;
            }
        }

        private ISurface EffectSurface
        {
            get;
            set;
        } = null!;

        bool IAction.Invoke(IList<byte> data)
        {
            this.newRoom = data[0];
            this.newPos = new Point(data[1], data[2]);
            this.count = 8;

            // Take a copy of the room.
            // TODO
            /*
            EffectSurface = _roomProvider.CreateCopy(
                new Rectangle(0, 
                    OffsetY, 
                    ZXSpectrum.Wares.PixelWidth,
                    ZXSpectrum.Wares.PixelHeight - OffsetY));
            */

            //_roomProvider.Visible = false;
            _player.Visible = false;
            _player.Position = new Point(data[1], data[2]);
            return true;
        }

        bool IUpdate.Update()
        {
            bool done = false;

            //EffectSurface.ClearByEffect();
            //game.Surface.Blit(EffectSurface, 0, OffsetY);
            this.count--;

            if (this.count == 0)
            {
                _roomProvider.SetRoom(newRoom);
                done = true;
                _player.Visible = true;
            }

            // Do once only.
            return done;
        }

       #region Buildable

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest(ClassNames.Wally, typeof(IPlayer));
            requests.AddRequest(ClassNames.RoomProvider, typeof(IRoomProvider));
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _player = dependencies.TryGetInstance<IPlayer>(ClassNames.Wally);
            _roomProvider = dependencies.TryGetInstance<IRoomProvider>(ClassNames.RoomProvider);
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

