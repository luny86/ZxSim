
namespace GameEditorLib.Builder
{
    /// <summary>
    /// A pool of objects that are other
    /// classes might have a dependency on.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface IDependencyPool
    {
        /// <summary>
        /// Add a new dependency
        /// </summary>
        /// <param name="scope">Full name of library holding instance.</param>
        /// <param name="type">
        /// The type the instance is based on or
        /// preferrably an interface it implements.
        /// </param>
        /// <param name="instance">Instance to store.</param>
        void Add(string scope, Type type, object instance);
    }
}