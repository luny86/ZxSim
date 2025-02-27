
using Builder;

namespace Bindings
{
    internal class Composition : IComposition, IBuildable
    {
        #region IComposition

        string IComposition.Name => "Bindings";

        #endregion

        #region IBuildable

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

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            dependencies.Add(ClassNames.BindingManager, typeof(IBindingManager), new BindingsManager());
        }

        #endregion
    }
}