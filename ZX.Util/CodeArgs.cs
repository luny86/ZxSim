
namespace ZX.Util
{
    /// <summary>
    /// Holds information on a data code.
    /// </summary>
    /// <remarks>
    /// Includes information on the data code,
    /// and the bytes that follow a code.
    /// Codes defines strings of data and 
    /// each code describes what to do with
    /// the data.
    /// </remarks>
    public class CodeArgs
    {
        public CodeArgs(byte[] args, CodeInfo info)
        {
            Args = args;
            Info = new CodeInfo(info);
        }

        public byte[] Args { get; }

        public CodeInfo Info { get; }
    }
}