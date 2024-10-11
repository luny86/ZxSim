
namespace Platform
{
    /// <summary>
    /// Main factory for platform objects
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// Creates a surface for manipulating and displaying native images
        /// </summary>
        /// <returns>ISurface object holding an image.</returns>
        static public ISurface CreateSurface() { return new Surface(); }
    }
}