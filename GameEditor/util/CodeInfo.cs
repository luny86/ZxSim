

namespace KUtil
{
        /// <summary>
        /// Information on each code, including how many
        /// bytes it uses and a descriptive string.
        /// </summary>
        public class CodeInfo
        {
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