
using ZX;
using ZX.Drawing;
using ZX.Platform;
using ZX.Util;

namespace Pyjamarama
{
    /// <summary>
    /// Draws trhe wall and ceiling for rooms that require it.
    /// </summary>
    internal class WallDrawer : IDrawer
    {
        #region Private Members

        private readonly IChunk _tiles;

        private readonly IDrawer _tileDrawer;

        private readonly IAttribute _attribute;
        #endregion

        #region Construction
        
        public WallDrawer(IDrawer tileDrawer, IChunk tiles)
        {
            _tileDrawer = tileDrawer;
            _tiles = tiles;
            _attribute = _tileDrawer as IAttribute 
                ?? throw new InvalidCastException("Tile drawer should implement IAttribute.");
        }

        #endregion

        #region IDrawer

        void IDrawer.Draw(ISurface surface, int index, int x, int y)
        {
            DrawCeiling(surface);
            DrawFloor(surface);
            DrawWalls(surface);
        }

        private void DrawCeiling(ISurface surface)
        {
            Palette.SetAttribute(0x43, _attribute);

            for(int i = 0; i < 0x20; i++)
            {
                _tileDrawer.Draw(surface, 0, i, 0x05);
            }
        }

        private void DrawFloor(ISurface surface)
        {
            Palette.SetAttribute(0x45, _attribute);

            for(int i = 0; i < 0x20; i++)
            {
                _tileDrawer.Draw(surface, 1, i, 0x17);
            }   
        }

        private void DrawWalls(ISurface surface)
        {
            Palette.SetAttribute(0x42, _attribute);

            for(int i = 0; i < 0x11; i++)
            {
                _tileDrawer.Draw(surface, 2, 0, i+6);
                _tileDrawer.Draw(surface, 2, 0x1f, i+6);
            }   
        }
        #endregion
    }
}