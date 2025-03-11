

using System.Drawing;
using Builder;

namespace Pyjamarama.House
{
    public class TestPosition : ITest, IBuildable
    {
        // Data indicies - Data held with logic commands.
        const int DataX = 0;
        const int DataY = 1;

        private IPlayer _player = null!;

        public TestPosition()
        {
        }

        int ITest.TestDataSize
        {
            get { return 2; }
        }

        bool ITest.Test(IList<byte> data)
        {
            bool passed = false;

            // TODO - maybe have a person provider instead...
            if (!_player.IsDead && !_player.Disabled)
            {
                // TODO - Move into person as part of interface.
                // This will allow multiple people when needed.
                Point p = _player.Position;
                int tx = (int)data[DataX];
                int ty = (int)data[DataY];

                if (p.X >= (tx - 8) && p.X < (tx + 8))
                {
                    if (p.Y >= (ty - 3) && p.Y < (ty + 3))
                    {
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

