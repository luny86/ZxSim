
namespace Builder
{
    /// <summary>
    /// Builder incorporates a derived Builder and Composite design patterns
    /// to allow class instance sharing.
    /// <para>
    /// This library creates a form of instance injection using interfaces
    /// which allows for easier unit testing.
    /// </para>
    /// <para>
    /// To use simply add:
    /// <code>
    ///     Creator creator = new Creator();
    ///     creator.BuildAll(this);	
    /// </code>
    /// at the earliest intialisation of your code.
    /// Where 'this' is an optional argument if you want the calling class
    /// to be included as an IBuilder implementation. See below.
    /// </para>
    /// <para>
    /// Any class the implements the <see cref="Builder.IComposition"/> class
    /// will be automatically instantiated by the creator. <see cref="IComposition.CreateBuildables()"/> 
    /// will be called for each instance allowing child instances that implement
    /// <see cref="Builder.IBuildable"/> to be included. 
    /// </para>
    /// <para>
    /// Any IBuildable instances will be collectively called as follows:
    /// <list>
    /// <item><see cref="IBuildable.RegisterObjects"/> will allow instances to be registered for injection.</item>
    /// <item><see cref="IBuildable.AskForDependents"/> allows the callee to request instances to be injected.</item>
    /// <item><see cref="IBuildable.DependentsMet"/> is finally called for all, passing in any requested instances.</item> 
    /// </list>
    /// All requests are optional so if not found they will not be in the dependency list, returning null if asked for.
    /// </para>
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {

    }
}