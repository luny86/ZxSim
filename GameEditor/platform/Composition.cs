
using System.Collections.Generic;
using Godot;
using GameEditorLib.Builder;

namespace Platform
{
    internal class Composition : IComposition, IBuildable
    {
        string IComposition.Name => "GameEditor.Platform.Composition";

        void IBuildable.AskForDependents(IRequests requests)
        {
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            dependencies.Add("GameEditorLib.Platform.IFactory", 
                typeof(GameEditorLib.Platform.IFactory),
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