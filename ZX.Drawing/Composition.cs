using GameEditorLib.Builder;

namespace ZX.Drawing
{
    /// <summary>
    /// Composite object for all ZX.Drawing instances.
    /// </summary>
    internal class Composition : IComposition, IBuildable
    {
        string IComposition.Name => "ZX.Drawing.Composition";

        void IBuildable.AskForDependents(IRequests requests)
        {
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            dependencies.Add("ZX.Drawing.IFactory", 
                typeof(ZX.Drawing.IFactory),
                new Factory());
        }

        IList<IBuildable> IComposition.CreateBuildables()
        {
            return null;
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
        }
    }
}