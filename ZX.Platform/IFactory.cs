
using ZX;

namespace ZX.Platform
{
    /// <summary>
    /// Describes the factory used to create
    /// platform dependent objects.
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Low level binary loader for platform.
        /// </summary>
        /// <param name="filePath">Path to file to load.</param>
        /// <returns>Bytes holding binary file data.</returns>
        byte[] LoadBinary(string filePath);

        /// <summary>
        /// Creates a platform dependent
        /// bitmap surface
        /// </summary>
        /// <returns>New surface object.</returns>
        ISurface CreateSurface();
    }
}
