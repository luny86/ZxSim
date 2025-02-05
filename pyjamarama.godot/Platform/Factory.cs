
using Godot;
using ZX.Platform;

namespace Platform
{
	/// <summary>
	/// Main factory for platform objects
	/// </summary>
	public class Factory : IFactory
	{
		public Factory() 
		{
		}

		byte[] IFactory.LoadBinary(string filePath)
		{
			using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);

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
			IView view = Main.Singleton.CreateScreen();
			view.Surface = CreateSurface();
			return view;
		}
	}
}
