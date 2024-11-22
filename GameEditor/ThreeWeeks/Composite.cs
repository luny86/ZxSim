
using GameEditorLib.Builder;
using GameEditorLib.Platform;
using System.Collections.Generic;

namespace ThreeWeeks;

public class Composite : IBuildable, IComposition
{
    public Composite()
    {
    }

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
		KUi.FurnitureDraw furniture = CPU.Instance.CreateFurnitureDraw(surface);

        dependencies.Add(typeof(KUi.FurnitureDrawer), furniture);
        Godot.GD.Print("Register...");
    }

    void IBuildable.AskForDependents()
    {
        Godot.GD.Print("Asking...");
    }

    void IBuildable.DependentsMet(IDependencyPool dependencies)
    {
        Godot.GD.Print("Met...");
    }
}
