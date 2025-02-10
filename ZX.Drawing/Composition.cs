using System.Runtime.CompilerServices;

using GameEditorLib.Builder;
[assembly:InternalsVisibleTo("ZX.Tests")]

namespace ZX.Drawing
{
    /// <summary>
    /// Composite object for all ZX.Drawing instances.
    /// </summary>
    internal class Composition : IComposition, IBuildable
    {
        string IComposition.Name => "ZX.Drawing.Composition";

        private Screen _screen = null!;

        private Factory _factory = null!;

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest("Platform.Main.IView", 
			    typeof(ZX.Platform.IView));
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            _screen = new Screen();

            dependencies.Add("ZX.Drawing.IFactory", 
                typeof(ZX.Drawing.IFactory),
                _factory);
            dependencies.Add("ZX.Drawing.Screen",
                typeof(ZX.Drawing.IScreen),
                _screen);
        }

        IList<IBuildable> IBuildable.CreateBuildables()
        {
            _factory = new Factory();

            return new List<IBuildable>()
            {
                _factory
            };
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {

        }
    }
}