
using System.Collections.Generic;

namespace ZX.Game
{
    /// <summary>
    /// Main top level object for handling all gaming aspects.
    /// </summary>
    internal class Provider : IGameProvider
    {   
        /// <summary>
        /// Objects used during a game.
        /// </summary>
        /// <remarks>This can be anything from flags to in-game sprites.</remarks>
        private readonly List<IGameStatic> _items = new List<IGameStatic>();

        public Provider()
        {

        }

        void IGameProvider.AddItem(IGameStatic item)
        {
            ArgumentNullException.ThrowIfNull(item);

            _items.Add(item);
        }

        void IGameProvider.Update()
        {
            foreach(IGameStatic staticItem in _items)
            {
                if(staticItem is IGameItem item)
                {
                    item.Update();
                }
            }
        }
    }
}