

namespace KUtil
{
    /// <summary>
    /// Signature for the method called when
    /// matching a code.
    /// </summary>
    /// <param name="args">Information on the code found.</param>
    public delegate void CodeMethod(CodeArgs args);

    /// <summary>
    /// Information on each code, including how many
    /// bytes it uses and a descriptive string.
    /// </summary>
    public class CodeInfo
    {
        public CodeInfo(CodeInfo copy)
        {
            Code = copy.Code;
            NumberOfArgs = copy.NumberOfArgs;
            Name = copy.Name;
        }

        public CodeInfo(byte code, int numArgs, string name)
        {
            Code = code;
            NumberOfArgs = numArgs;
            Name = name;
        }

        public byte Code { get; }
        // Including initial code
        public int NumberOfArgs { get; }
        public string Name { get; }
    }
}