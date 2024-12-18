using System;

namespace KUi
{
    /// <summary>
    /// Arguments used when signalling a tile index change.
    /// </summary>
    public class SelectTileEventArgs : EventArgs
    {
        public SelectTileEventArgs(int index)
        {
            Index = index;
        }
        
        public int Index { get; }
    }
}
	
