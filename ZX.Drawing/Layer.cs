
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// Defines a draw layer which describes how surface should be blitted
    /// to the main screen. Override <see cref="Update"/> to handle
    /// how things are drawn etc.
    /// </summary>
    public abstract class Layer : ILayer, IEquatable<ILayer>, IComparable<ILayer>, IComparable
    {
        public Layer(string name, ISurface surface, int z)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surface = surface;
            Z = z;
            X = 0;
            Y = 0;
        }

        public string Name { get; }

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
            else if(obj is ILayer layer)
            {
                return Equals(layer);
            }

            return false;
        }

        public bool Equals(ILayer? other)
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

        public int CompareTo(ILayer? layer)
        {
            if(layer == null)
                return 1;
            else
                return Z.CompareTo(layer.Z);
        }

        public int CompareTo(object? obj)
        {
            if(obj is ILayer layer)
            {
                CompareTo(layer);
            }

            return 1;
        }
        #endregion
    }
}