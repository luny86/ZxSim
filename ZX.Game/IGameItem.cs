
namespace ZX.Game
{
    /// <summary>
    /// A <see cref="IGameItem"/> is a physical game object
    /// that needs to be updated but also requires the 
    /// <see cref="IGameStatic"/> properties for new games and levels. 
    /// </summary>
    /// <seealso cref="IGameStatic"/>
    public interface IGameItem : IGameStatic
    {
        /// <summary>
        /// Updates the object ready for the next rendering.
        /// </summary>
        public void Update();
    }
}