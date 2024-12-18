
namespace GameEditorLib
{
    /// <summary>
    /// Data class for holding information on
    /// a dependency request.
    /// </summary>
    internal class Request
    {
        public Request(string scope, Type type)
        {
            Scope = scope;
            Type = type;
        }

        public string Scope { get; }
        public Type Type { get; }

        public override string ToString()
        {
            return $"Request : {Scope}, {Type.Name}";
        }
    }
}