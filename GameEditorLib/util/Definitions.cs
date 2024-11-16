
namespace KUtil
{
    /// <summary>
    /// Signature for methods used when handling special codes
    /// within a data chunk enumerator.
    /// </summary>
    /// <remarks>
    /// the idea is to create a map of methods linked to a command
    /// defined by the byte data that is being iterated. A 
    /// matching code will invoke the method allowing
    /// the enumerator index to be altered.
    /// </remarks>
    /// <param name="index">current index of enumerator.</param>
    /// <param name="dataGroup">Information of data being scanned</param>
    /// <returns>Current index or altered index.</returns>
    public delegate int EnumeratorAlterMethod(int index, FurnitureDataGroup dataGroup);
}