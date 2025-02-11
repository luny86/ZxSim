
namespace ZX.Game
{
    /// <summary>
    /// Realises an object which requires intialisation or changes during a 
    /// game cycle, such as a new game or new level.
    /// </summary>
    public interface IGameItem
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