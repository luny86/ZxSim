using System.Collections.Generic;

namespace KUtil
{
    /// <summary>
    /// Holds all the different pieces of data required
    /// when handling the Furniture container and
    /// drawer.
    /// </summary>
    public class FurnitureDataGroup
    {
        /// <summary>
        /// Chunk of memory holding all the furniture
        /// description strings.
        /// </summary>
        public IChunk? FurnitureData { get; init; }
        /// <summary>
        /// dictionary holding the mappings between a 
        /// code and it's information.
        /// </summary>
        public IReadOnlyDictionary<byte, CodeInfo>? CodeInfoMapping { get; init; }
        /// <summary>
        /// List of byte ranges that represent
        /// a single type of code.
        /// </summary>
        public IReadOnlyList<Range>? CodeRanges { get; init; }

        /// <summary>
        /// Map of methods that will be invoked, from within the enumerator
        /// if the current code matches.
        /// </summary>
        public IReadOnlyDictionary<byte, EnumeratorAlterMethod>?  SpecialCodes { get; init; }
    }
}