
namespace ZX.Game
{
    /// <summary>
    /// Basic flag which holds an integer value and can be related to a game 'object' 
    /// by index.
    /// </summary>
    internal class Flag : IFlag
    {
        public const int NoObject = -1;

        public Flag(int value)
        {
            Value = value;
            Object = NoObject;
        }

        public Flag(int value, int relatedObjectIndex)
        {
            Value = value;
            Object = relatedObjectIndex;
        }

        public int Value
        {
            get;
            set;
        }

        /// <summary>
        /// Links an object to the flag.
        /// </summary>
        /// <value>The object.</value>
        public int Object
        {
            get;
            private set;
        }
    }
}