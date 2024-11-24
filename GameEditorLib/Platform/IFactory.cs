
namespace GameEditorLib.Platform
{
    /// <summary>
    /// Describes the factory used to create
    /// platform dependent objects.
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Creates a platform dependent
        /// bitmap surface
        /// </summary>
        /// <returns>New surface object.</returns>
        public ISurface CreateSurface();
    }
}
