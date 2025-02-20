
using System.Collections.Generic;
using Builder;
using ZX.Platform;

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
			dependencies.Add(ClassNames.Factory, 
				typeof(ZX.Platform.IFactory),
				new Factory());
			dependencies.Add(ClassNames.UserInput,
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
