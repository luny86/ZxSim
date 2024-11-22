
using GameEditorLib.Platform;

namespace Platform
{
    /// <summary>
    /// Main factory for platform objects
    /// </summary>
    public class Factory : IFactory
    {
        /// <summary>
        /// Creates a surface for manipulating and displaying native images
        /// </summary>
        /// <returns>ISurface object holding an image.</returns>
        public ISurface CreateSurface() { return new Surface(); }
    }
}
