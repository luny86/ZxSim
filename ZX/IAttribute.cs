
using ZX.Util;

namespace ZX
{
    /// <summary>
    /// Describes an object that can hold RGBA values
    /// of the ZX Spectrum colours as Paper and Ink.
    /// </summary>
    public interface IAttribute
    {
        /// <summary>
        /// Rgba version of a 'paper' colour.
        /// </summary>
        Rgba Paper {get; set;}

        /// <summary>
        /// Rgba version of an 'ink' colour.
        /// </summary>
        Rgba Ink {get; set;}
    }
}