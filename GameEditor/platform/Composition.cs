
using System.Collections.Generic;
using Godot;
using GameEditorLib.Builder;

namespace Platform
{
    internal class Composition : IComposition, IBuildable
    {
        // True address for start of ram in buffer.
        private int _startAddr = 0x4000;

    	private byte[] _ram;

        string IComposition.Name => "GameEditor.Platform.Composition";

        void IBuildable.AskForDependents(IRequests requests)
        {
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            CreateMemory();

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

        private void CreateMemory()
        {
            using var file = FileAccess.Open("res://game.bin", FileAccess.ModeFlags.Read);
            _ram = file.GetBuffer(49152);
        }
    }
}