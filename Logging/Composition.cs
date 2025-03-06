using System.Runtime.CompilerServices;

using Builder;
[assembly:InternalsVisibleTo("LoggingTests")]

namespace Logging
{
    /// <summary>
    /// Composite object for all logging.
    /// </summary>
    internal class Composition : IComposition, IBuildable
    {
        private Factory _factory = null!;

        string IComposition.Name => "Logging";

        #region IBuildable
        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            Settings settings = new Settings();
            _factory = new Factory(settings);
            dependencies.Add(ClassNames.Factory, typeof(IFactory), _factory);
        }

        void IBuildable.AskForDependents(IRequests requests)
        {

        }

        IList<IBuildable> IBuildable.CreateBuildables()
        {
            return new List<IBuildable>();
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