
namespace ZX.Game
{
    /// <summary>
    /// Defines the main game provider for all game running espects.
    /// </summary>
    public interface IGameProvider
    {
        /// <summary>
        /// Add an item to the game.
        /// </summary>
        /// <remarks>
        /// Although all items are to be base on <see cref="IGameStatic"/>
        /// they can implement other interfaces, such as <see cref="IGameItem"/>
        /// in order to define other attributes recognised by the <see cref="IGameProvider"/> 
        /// </remarks>
        /// <seealso cref="IGameItem"/> 
        void AddItem(IGameStatic item);

        /// <summary>
        /// Update all non-static items.
        /// </summary>
        void Update();
    }
}