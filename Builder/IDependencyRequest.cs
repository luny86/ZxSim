
namespace GameEditorLib.Builder
{
    /// <summary>
    /// Implements the interface to get requests
    /// for other objects from the builder.
    /// </summary>
    public interface IDependencyRequest
    {
        void Request(string scope, Type type);
    }
}