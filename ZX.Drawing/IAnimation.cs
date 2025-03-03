

using System.Drawing;

namespace ZX.Drawing
{
    /// <summary>
    /// Descibes an animated object.
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// Gets the unique name of the animation.
        /// </summary>
        string Name
        {
            get;
            init;
        }

        /// <summary>
        /// If set, then the animation is completed
        /// and needs to be removed.
        /// </summary>
        bool Completed
        {
            get;
        }

        /// <summary>
        /// Determines if the animation is active or not.
        /// If not then it will not be drawn or updated.
        /// </summary>
        bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the animation keeps looping or
        /// stops animating when the <see cref="LastFrame"/> 
        /// has been reached.
        /// </summary>
        bool Loop
        {
            get;
            set;
        }

        /// <summary>
        /// Current frame that should be displayed.
        /// </summary>
        int Frame
        {
            get;
            init;
        }

        /// <summary>
        /// First frame in animation.
        /// </summary>
        int StartFrame
        {
            get;
            init;
        }

        /// <summary>
        /// Gets the last frame in animation.
        /// </summary>
        int LastFrame
        {
            get;
            init;
        }

        /// <summary>
        /// Gets or sets the number of game cycles
        /// between each animated frame.
        /// </summary>
        int Frequency
        {
            get;
            init;
        }

        /// <summary>
        /// Gets the on layer position.
        /// </summary>
        Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// Update animation and position as required.
        /// </summary>
        void Update();
    }
}
