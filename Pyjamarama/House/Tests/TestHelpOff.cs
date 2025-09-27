
using Builder;
using ZX.Game;

namespace Pyjamarama.House
{
    public class TestHelpOff : ITest, IBuildable
    {
        private IFlags _flags = null!;

        public TestHelpOff()
        {
        }

        #region ITest implementation

        public bool Test(System.Collections.Generic.IList<byte> data)
        {
            return _flags[FlagsNames.HelpSwitch].Value == 0;
        }

        public int TestDataSize
        {
            get
            {
                return 0;
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
            requests.AddRequest("ZX.Game.Flags", typeof(IFlags));
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _flags = dependencies.TryGetInstance<IFlags>("ZX.Game.Flags");
        }

        void IBuildable.EndBuild()
        {
        }
        #endregion
    }
}

