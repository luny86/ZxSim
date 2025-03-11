
using System.Drawing;

namespace Pyjamarama
{
    public interface IPlayer
    {
        bool Disabled { get; }

        bool Visible { get; set; }

        bool IsDead { get; }
        
        Point Position { get; set; }

        bool JustPickedUp { get; set; }
    }
}