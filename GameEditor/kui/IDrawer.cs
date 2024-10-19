using Platform;

namespace KUi
{
    /// <summary>
    /// Basic interface for an item drawer.
    /// </summary>
    public interface IDrawer
    {
        int TileStart { get; }
        
        void SetTileStart(int address);

        void Draw(int x, int y, int index, ISurface surface);
    }
}