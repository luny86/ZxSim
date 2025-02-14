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
            dependencies.Add(ClassNames.Flags, typeof(ZX.Game.IFlags), new Flags());
            dependencies.Add(ClassNames.Factory, typeof(ZX.Game.IFactory), new Factory());
            dependencies.Add(ClassNames.GameProvider, typeof(ZX.Game.IGameProvider), new Provider());
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

        void IBuildable.EndBuild()
        {

        }
        #endregion


    }
}