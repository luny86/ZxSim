
using System.Drawing;

namespace Pyjamarama
{
    public interface IPlayer
    {
        bool Disabled { get; set; }

        bool Visible { get; set; }

        bool IsDead { get; }
        
        Point Position { get; set; }

        bool JustPickedUp { get; set; }

        int Frame { get; set; }

        /// <summary>
        /// Set player to be falling.
        /// </summary>
        /// <returns></returns>
        void Falling();
    }
}