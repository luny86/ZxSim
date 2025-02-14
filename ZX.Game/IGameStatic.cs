
namespace ZX.Game
{
    /// <summary>
    /// <see cref="IGameStatic"/> is a game object that does not get
    /// displayed or is physical, but is used throughout the game
    /// and needs to change for a new game or a new level.
    /// </summary>
    public interface IGameStatic
    {
        /// <summary>
        /// Invoke when a new game starts.
        /// </summary>
        /// <remarks>Method should handle any initialisation.</remarks>
        void NewGame();

        /// <summary>
        /// Invoke when a new level starts.
        /// </summary>
        /// <remarks>
        /// Method should handle any resets required when
        /// a new level starts.
        /// </remarks>
        void NewLevel();
    }
}