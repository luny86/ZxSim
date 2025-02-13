
namespace GameEditorLib.Builder;

/// <summary>
/// Interfac describing a buildable object.
/// </summary>
/// <remarks>
/// Buildable objects are automatically detected by
/// the Game Editor system and the methods called
/// during initialisation.
/// All implentation must have a default constructor
/// to be created by the build system.
/// </remarks>
public interface IBuildable
{        
    /// <summary>
    /// This member should create any child
    /// IBuildable instances belonging the to parent.
    /// </summary>
    /// <returns>List of buildables.</returns>
    IList<IBuildable>? CreateBuildables(); 

    /// <summary>
    /// Register objects owned by this instance
    /// for use by other buildables. This is 
    /// invoked first for all buildable objects.
    /// </summary>
    void RegisterObjects(IDependencyPool dependencies);

    /// <summary>
    /// Ask for other objects that the buildable
    /// is dependent on. This is invoked once all
    /// other objects have registered their objects.
    /// </summary>
    /// <param name="requests">Collection of requests that can be added to</param>
    void AskForDependents(IRequests requests);

    /// <summary>
    /// Invoked once all registration and dependencies
    /// have been met by all objects. 
    /// </summary>
    /// <remarks>
    /// Use this method to get references to any dependencies
    /// requested.
    /// </remarks>
    void DependentsMet(IDependencies dependencies);

    /// <summary>
    /// Final call to build.
    /// </summary>
    /// <remarks>
    /// This call allows any depedencies to be used wihtout fear of
    /// any race conditions that might be met if any are used
    /// earlier.
    /// </remarks>
    void EndBuild();
}
