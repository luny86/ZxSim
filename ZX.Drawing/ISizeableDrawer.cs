

using ZX.Util;

namespace ZX.Drawing
{
    /// <summary>
    /// Describes an <see cref="IDrawer"/>  that is able to have variable drawing dimensions
    /// regardless of bitmap dimensions.
    /// </summary>
    /// <see cref="IDrawer"/> 
    public interface ISizeableDrawer : IDrawer
    {
        /// <summary>
        /// Determines if the draw should erase the earea
        /// prior to drawing.
        /// </summary>
        bool PreClear { get; set; }

        /// <summary>
        /// Gets and sets the area define for drawing within the drawers
        /// area.
        /// </summary>
        Rectangle BlitRect { get; set; }
    }
}