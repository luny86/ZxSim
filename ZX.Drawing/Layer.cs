
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// Defines a draw layer which describes how surface should be blitted
    /// to the main screen. Override <see cref="Update"/> to handle
    /// how things are drawn etc.
    /// </summary>
    public abstract class Layer : ILayer, IEquatable<Layer>, IComparable<Layer>
    {
        public Layer(ISurface surface, int z)
        {
            Surface = surface;
            Z = z;
            X = 0;
            Y = 0;
        }

        protected ISurface Surface { get; }

        public int Z { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public abstract void Update();

        public void Blend(ISurface to)
        {
            Surface.Blend(to, X, Y);
        }

        #region Sorting
        public override bool Equals(object? obj)
        {
            if(obj is null)
            {
                return false;
            }
            else if(obj is Layer layer)
            {
                return Equals(layer);
            }

            return false;
        }

        public bool Equals(Layer? other)
        {
            if(other is null)
            {
                return false;
            }

            return Z.Equals(other.Z);
        }

        public override int GetHashCode()
        {
            return Z;
        }

        public int CompareTo(Layer? layer)
        {
            if(layer == null)
                return 1;
            else
                return Z.CompareTo(layer.Z);
        }
        #endregion
    }
}