
using Builder;
using ZX.Game;

namespace Pyjamarama.House
{
    public class ActionHelpOff : IAction, IBuildable
    {
        private IFlags _flags = null!;

        public ActionHelpOff()
        {
        }

        #region IAction implementation

        bool IAction.Invoke(System.Collections.Generic.IList<byte> data)
        {
            _flags[FlagsNames.HelpSwitch].Value = 0;
            return true;
        }

        public int DataSize
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

