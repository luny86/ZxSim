
using System.Collections.Generic;
using GameEditorLib.Builder;

namespace Platform
{
	internal class Composition : IComposition, IBuildable
	{
		string IComposition.Name => "Pyjarama.Platform.Composition";

		void IBuildable.AskForDependents(IRequests requests)
		{
		}

		void IBuildable.RegisterObjects(IDependencyPool dependencies)
		{
			dependencies.Add("ZX.Platform.IFactory", 
				typeof(ZX.Platform.IFactory),
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
