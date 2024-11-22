using Platform;
using GameEditorLib.Platform;

namespace KUi
{
	/// <summary>
	/// Basic interface for an item drawer.
	/// </summary>
	public interface IDrawer
	{
		void Draw(int x, int y, int index, ISurface surface);
	}
}
