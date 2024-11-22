
namespace GameEditorLib.Builder
{
    /// <summary>
    /// Readonly mapping of any dependencies found and
    /// passed onto the user.
    /// </summary>
    public interface IDependencies
    {
        /// <summary>
        /// Attempts to get an object based on it's
        /// full scope name and type from the 
        /// dependency pool.
        /// </summary>
        /// <param name="scope">Full scoping name of instance.</param>
        /// <param name="type">Type instance implements.</param>
        /// <returns>Either an existing instance of null if not found.</returns>
        object? TryGetInstance(string scope, Type type);
    }
}
