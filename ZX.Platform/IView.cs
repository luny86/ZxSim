

namespace ZX.Platform
{
    /// <summary>
    /// Describes a platforms view. This holds the main surface of
    /// a command for drawing the editorwindow to.
    /// </summary>
    public interface IView
    {
        ISurface Surface { get; set; }
    }
}