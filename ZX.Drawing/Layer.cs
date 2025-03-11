
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// Defines a draw layer which describes how surface should be blitted
    /// to the main screen. Override <see cref="Update"/> to handle
    /// how things are drawn etc.
    /// </summary>
    /// <remarks>
    /// Comparisons are done using the Z order value.
    /// </remarks>
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

        /// <summary>
        /// Name of layer.
        /// </summary>
        public string Name { get; }

        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets the surface that holds the image
        /// of the layer.
        /// </summary>
        /// <remarks>
        /// THis is the surface that is finally blended
        /// onto the main surface and should be used
        /// to draw to when updating the image of the
        /// layer.
        /// </remarks>
        protected ISurface Surface { get; }

        public int Z { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Called when the image needs redrawing or changing.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Call when the layer needs to be displayed
        /// onto a main surface such as the <see cref="IScreen"/> 
        /// </summary>
        /// <param name="to">Surface to blend to.</param>
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

            return Equals(other);
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
                return CompareTo(layer);
            }

            return 1;
        }
        #endregion
    }
}