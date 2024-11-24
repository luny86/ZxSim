
using GameEditorLib.Builder;
using GameEditorLib.Platform;
using System.Collections.Generic;

namespace ThreeWeeks;

public class Composite : IBuildable, IComposition
{
    public Composite()
    {
    }

    string IComposition.Name => "ThreeWeeks";

    IList<IBuildable> IComposition.CreateBuildables()
    {
        Godot.GD.Print("Child...");
        return null;
    }

    void IBuildable.RegisterObjects(IDependencyPool dependencies)
    {
        // TODO - Embed in platform - Get factory from dependencies
        // ah ha - circular - think about it!
        // Got it, this is a IView thing.....
        IFactory factory = new Platform.Factory();
        ISurface surface = factory.CreateSurface();

		// TODO - Node to deal surface.Updated += Surface_Updated;
		//KUi.FurnitureDraw furniture = CPU.Instance.CreateFurnitureDraw(surface);

        //dependencies.Add(typeof(KUi.FurnitureDrawer), furniture);
        Godot.GD.Print("Register...");
    }

    void IBuildable.AskForDependents(IRequests requests)
    {
        requests.AddRequest("GameEditorLib.Platform.IFactory", typeof(IFactory));
        Godot.GD.Print("Asking...");
    }

    void IBuildable.DependentsMet(IDependencies dependencies)
    {
        IFactory factory = 
            dependencies.TryGetInstance("GameEditorLib.Platform.IFactory", typeof(IFactory))
            as IFactory;

        Godot.GD.Print($"Met...{factory}");
    }
}
