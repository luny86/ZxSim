
using GameEditorLib.Builder;

namespace Pyjamarama
{
    internal class Composition : IComposition, IBuildable
    {

        private Factory _factory = new Factory();

        string IComposition.Name => "Pyjamarama";

        void IBuildable.AskForDependents(IRequests requests)
        {
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return new List<IBuildable>()
            {
                _factory
            };
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            dependencies.Add("Pyjamarama.Factory", typeof(IFactory), _factory);
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
        }

    }
}