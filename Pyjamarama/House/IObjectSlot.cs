
using ZX.Game;

namespace Pyjamarama.House
{
    /// <summary>
    /// Interface describing an object slot,
    /// used by the rooms to hold an object
    /// that can be picked up by the hero.
    /// </summary>
    public interface IObjectSlot : IGameStatic
    {
        // Position of object, on screen.
        int X { get; }
        int Y { get; }

        // Index of object slot is currently holding.
        int ObjectIndex
        {
            get;
            set;
        }
    }
}