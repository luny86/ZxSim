
using System.Collections;

namespace ZX.Util
{
    /// <summary>
    /// Holds game data that is made up of data string
    /// arrays of variable length.
    /// </summary>
    public interface IDataContainer
    {
        IEnumerable this[int index] { get; }
    }
}