
using Godot;
using GameEditorLib.Platform;

namespace Platform
{
    /// <summary>
    /// Main factory for platform objects
    /// </summary>
    public class Factory : IFactory
    {
        public Factory() {}

        byte[] IFactory.LoadBinary(string filePath)
        {
            using var file = FileAccess.Open("res://game.bin", FileAccess.ModeFlags.Read);

            byte[] ram = file.GetBuffer(0x10000);
            return ram;
        }

        /// <summary>
        /// Creates a surface for manipulating and displaying native images
        /// </summary>
        /// <returns>ISurface object holding an image.</returns>
        public ISurface CreateSurface() { return new Surface(); }

        public IView CreateCommand(string name)
        {
            IView view = CPU.Instance.CreateCommand(name);
            view.Surface = CreateSurface();
            return view;
        }
    }
}
