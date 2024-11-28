
using GameEditorLib.zx;

namespace GameEditorLib.Platform
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

        /// <summary>
        /// Creates an icon/view command on the platform.
        /// </summary>
        /// <param name="name">Name of command</param>
        /// <returns>Reference to the view created by the command.</returns>
        IView CreateCommand(string name);
    }
}
