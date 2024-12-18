
namespace GameEditorLib.Ui
{
    public interface IBoundsDraw
    {
        bool DrawnOutOfBounds { get; }
        public System.Drawing.Point OutOfBoundsHit { get; }
    }
}