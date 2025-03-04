
using System.Drawing;

namespace Pyjamarama
{
    public interface IPlayer
    {
        bool Disabled { get; }

        bool IsDead { get; }
        
        Point Position { get; }

        bool JustPickedUp { get; }
    }
}