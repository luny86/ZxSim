
using KUtil;

namespace zx
{
    /// <summary>
    /// Describes an object that can hold RGBA values
    /// of the ZX Spectrum colours as Paper and Ink.
    /// </summary>
    public interface IAttribute
    {
        public Rgba Paper {get; set;}
        public Rgba Ink {get; set;}
    }
}