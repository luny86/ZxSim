using System;
using System.Drawing;

namespace KUtil
{
    /// <summary>
    /// Keeps a cache of positions, indexed.
    /// </summary>
    /// <remarks>
    /// current positions can be checked to see if they are 
    /// out of bounds and positions can be updated.
    /// All start positions are set to middle of bounds
    /// as a starting point.
    /// </remarks>
    public class DrawPositionCache
    {
        // Nmber of positions cached.
        private readonly int _size;
        // Bounding area for testing out of bounds.
        private readonly Rectangle _bounds;
        // Cache of positions.
        private Point[] _positions;
        // Amount to shift by when out of bounds.
        private readonly int _gridSize;

        public DrawPositionCache(Rectangle bounds, int size, int gridSize)
        {
            _size = size;
            _bounds = bounds;
            _gridSize = gridSize;
            _positions = new Point[_size];

            for(int i=0; i<_size; i++)
            {
                _positions[i] = new Point(0,0);
            }
        }

        public Point this[int index]
        {
            get
            {
                return _positions[index];
            }
        }

        /// <summary>
        /// Test is current position is in bounds.
        /// </summary>
        /// <remarks>
        /// If any position is not in bounds then the
        /// cached start position is shifted by the grid size.
        /// This is indicated by a false return value.
        /// </remarks>
        /// <param name="index">Start position to work on</param>
        /// <param name="x">Current draw position.</param>
        /// <param name="y">Current draw position.</param>
        /// <returns></returns>
        public bool IsInBounds(int index, int x, int y)
        {
            bool inBounds = true;

            if(x < _bounds.Left)
            {
                _positions[index].X += _gridSize;
                inBounds = false;
            }

            if(x > _bounds.Right)
            {
                _positions[index].X -= _gridSize;
                inBounds = false;
            }

            if(y < _bounds.Top)
            {
                _positions[index].Y += _gridSize;
                inBounds = false;
            }

            if(y > _bounds.Bottom)
            {
                _positions[index].Y -= _gridSize;
                inBounds = false;
            }

            return inBounds;
        }
    }
}