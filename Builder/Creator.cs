

using System.Text;
using System.Runtime.CompilerServices;
[assembly:InternalsVisibleTo("ZX.Tests")]


namespace Builder;

/// <summary>
/// Main creator object for the builder engine.
/// </summary>
public class Creator : IDisposable
{
    #region Private Types
    private class ScopedInfo : IRequests
    {
        public ScopedInfo()
        {
            Requests = new List<Request>();
            Instances = new DependencyPool();
            ChildBuildables = new List<IBuildable>();
        }

        public List<Request> Requests { get; }
        public DependencyPool Instances { get; }
        public List<IBuildable> ChildBuildables { get; }

        public void AddRequest(string scope, Type type)
        {
            Requests.Add(new Request(scope, type));
        }

        public void AddChildRange(List<IBuildable> children)
        {
            ChildBuildables.AddRange(children);
        }

        public override string ToString()
        {
            return $"ScopedInfo(Requests = {Requests.Count}, Instances = {Instances}";
        }
    }
    #endregion
    
    #region Information
    const string CompositionTypeName = "Builder.IComposition";
    #endregion

    #region Members
    // List of all classes based on IBuildable from in the runtime.
    private readonly List<IBuildable> _buildables = new List<IBuildable>();

    // Holds all of the registered instances for global use.
    private readonly DependencyPool _instancePool = new DependencyPool();

    // Holds a map of an instances dependency requests and
    // finally a list of instances that meet those requests.
    private readonly Dictionary<IBuildable, ScopedInfo> _scopedInfo = 
        new Dictionary<IBuildable, ScopedInfo>();

    // Map of the compositions found, by name.
    public Dictionary<string, IComposition> _compositions =
        new Dictionary<string, IComposition>();
    #endregion

    #region Construction
    /// <summary>
    /// Creates an instance of <see cref="Creator"/> 
    /// </summary>
    /// <param name="preload">If true then it will attempt to preload library DLLs.</param>
    public Creator(bool preload = false)
    {
        if(preload)
        {
            PreloadAssemblies();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool _)
    {
        foreach(IBuildable build in _buildables)
        {
            if(build is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _buildables.Clear();
    }
    #endregion 

    #region Public Helpers
    public IComposition? TryGetComposition(string name)
    {
        IComposition? found = null;
        
        if(!string.IsNullOrWhiteSpace(name))
        {
            _compositions.TryGetValue(name, out found);
        }

        return found;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(_instancePool.ToString());
        sb.AppendLine(_scopedInfo.ToString());

        sb.AppendLine("Buildables");
        foreach(IBuildable build in _buildables)
            sb.AppendLine($" - {build}");

        sb.AppendLine("IComposites");
        foreach(var a in _compositions)
        {
            sb.AppendLine($" -- {a.Key} {a.Value.Name}");
        }
        return sb.ToString();
    }
    #endregion

    #region Build members
    public void BuildAll()
    {
        _buildables.Clear();
        BuildAllInternal();
    }

    public void BuildAll(IBuildable initialBuildable)
    {
        _buildables.Clear();
        _buildables.Add(initialBuildable);
        AddChildBuildables(initialBuildable);
        BuildAllInternal();
    }

    private void BuildAllInternal()
    {
        FindAllBuildables();
        RegisterObjectsFromBuildables();
        AskForDependents();
        BuildScopedDependencyPools();
        RunDependentsMet();
        EndBuild();
    }

    private void FindAllBuildables()
    {
        IList<IComposition> compositions = Compositions.ToList();
        foreach(IComposition composition in compositions)
        {
            _compositions.Add(composition.Name, composition);

            if(composition is IBuildable buildable)
            {
                _buildables.Add(buildable);
                AddChildBuildables(buildable);
            }
        }
    }

    private void AddChildBuildables(IBuildable parent)
    {
        if(parent != null)
        {
            IList<IBuildable>? buildables = parent.CreateBuildables();

            if(buildables != null)
            {
                _buildables.AddRange(buildables);
                foreach(IBuildable inner in buildables)
                {
                    AddChildBuildables(inner);
                }
            }
        }
    }

    private void RegisterObjectsFromBuildables()
    {
        foreach(IBuildable build in _buildables)
        {
            build.RegisterObjects(_instancePool);
        }

    }

    private void AskForDependents()
    {
        foreach(IBuildable build in _buildables)
        {
            ScopedInfo requests = new ScopedInfo();
            _scopedInfo.Add(build, requests);

            build.AskForDependents(requests);
        }
    }

    private void EndBuild()
    {
        foreach(IBuildable build in _buildables)
        {
            build.EndBuild();
        }
    }

    /// <summary>
    /// Lets all buildables know the dependencies have been met
    /// and supply a list of available to the buildable.
    /// </summary>
    /// <exception cref="NullReferenceException">
    /// If a buildable holds a null scoped info.
    /// </exception>
    private void RunDependentsMet()
    {
        foreach(IBuildable build in _buildables)
        {
            _scopedInfo.TryGetValue(build, out ScopedInfo? info);
            if(info == null)
            {
                throw new NullReferenceException(
                    "Creator has found a buildable that does not have any scoped info.");
            }
            else
            {
                build.DependentsMet(info.Instances);
            }
        }
    }

    /// <summary>
    /// Run through each request and add any found in
    /// the global instance pool into the scoped
    /// list for a specific buildable.
    /// </summary>
    private void BuildScopedDependencyPools()
    {
        foreach(var pair in _scopedInfo)
        {
            foreach(Request request in pair.Value.Requests)
            {
                if((_instancePool as IDependencies).TryGetInstance(
                    request.Scope, request.Type) is object o)
                {
                    (pair.Value.Instances as IDependencyPool).Add(request.Scope, request.Type, o);
                }
                else
                {
                    //throw new InvalidOperationException($"Unable to find {request.Scope} of type {request.Type.FullName}");
                }
            }
        }
    }

    private static void PreloadAssemblies()
    {
        foreach(var refAssembly in
            AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany( b => b.GetReferencedAssemblies())
            .Where(b => b.Name != null && b.Name.StartsWith("ZX")))
            {
                System.Reflection.Assembly.Load(refAssembly);
            }
    }

    public static IEnumerable<IComposition> Compositions
    {
        get
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                .Where(m => m.IsClass && m.GetInterface(CompositionTypeName) != null)
                select Activator.CreateInstance(t) as IComposition;
        }
    }
    #endregion
}