
namespace Builder
{
    /// <summary>
    /// Implements the composition pattern which
    /// tells the builder to use and create a single
    /// instance of the class for the application.
    /// </summary>
    /// <remarks>
    /// Each time an implementation is found, the builder
    /// system will create and hold a single instance of
    /// the class. 
    /// If this class implements the IBuildable interface
    /// it will be added to the build process.
    /// </remarks>
     public interface IComposition
    {
        /// <summary>
        /// Unique name for composition.
        /// </summary>
        string Name { get; }
    }
}