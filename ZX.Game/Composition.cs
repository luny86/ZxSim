using GameEditorLib.Builder;

namespace ZX.Game
{
    internal class Composition : IComposition, IBuildable
    {
        #region IComposition

        string IComposition.Name => "ZX.Game.Composition";

        #endregion

        #region IBuildable

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            dependencies.Add("ZX.Game.Flags", typeof(ZX.Game.IFlags), new Flags());
            dependencies.Add("ZX.Game.Factory", typeof(ZX.Game.IFactory), new Factory());
        }

        void IBuildable.AskForDependents(IRequests requests)
        {
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
        }

        #endregion


    }
}