
using GameEditorLib.Platform;

namespace GameEditorLib.Ui
{
	/// <summary>
	/// Basic interface for an item drawer.
	/// </summary>
	public interface IDrawer
	{
		void Draw(int x, int y, int index, ISurface surface);
	}
}
