
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// Describes a graohpical layer that can be drawn onto an <see cref="ISurface"/> 
    /// or the <see cref="Screen"/>. It is up to the concrete class to determine how
    /// the layer image is created and sent to the surface. Normally a layer will
    /// have its own <see cref="ISurface"/> and user <see cref="IDrawer"/> objects.  
    /// </summary>
    public interface ILayer
    {
        /// <summary>
        /// Name of layer, used for retrieving.
        /// </summary>
        string Name { get; }

        int Z { get; }
        
        /// <summary>
        /// Determines if the layer is shown or not
        /// </summary>
        bool Visible { get; set; }
        
        /// <summary>
        /// Handle any actual drawing here.
        /// </summary>
        void Update();

        /// <summary>
        /// Blends the layers image onto a given surface.
        /// </summary>
        /// <param name="to"><see cref="ISurface"/> to copy or blend to.</param>
        void Blend(ISurface to);     
    }
}