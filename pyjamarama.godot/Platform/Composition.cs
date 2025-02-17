
using System.Collections.Generic;
using Builder;

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
			dependencies.Add("ZX.Platform.UserInput",
				typeof(ZX.Platform.IUserInput),
				new UserInputBridge());
		}

		IList<IBuildable> IBuildable.CreateBuildables()
		{
			return null;
		}

		void IBuildable.DependentsMet(IDependencies dependencies)
		{
		}
		
		void IBuildable.EndBuild()
		{
			
		}
	}
}
