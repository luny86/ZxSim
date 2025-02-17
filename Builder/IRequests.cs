
namespace Builder
{
    /// <summary>
    /// Holds a list of requests reuired by a buildable.
    /// </summary>
    public interface IRequests
    {
        void AddRequest(string scope, Type type);
    }
}