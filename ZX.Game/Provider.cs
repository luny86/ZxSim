
using System.Collections.Generic;

namespace ZX.Game
{
    /// <summary>
    /// Main top level object for handling all gaming aspects.
    /// </summary>
    internal class Provider : IGameProvider
    {   
        private enum State
        {
            None,
            NewGame,
            GamePlay,
            GameOver
        };

        /// <summary>
        /// Objects used during a game.
        /// </summary>
        /// <remarks>This can be anything from flags to in-game sprites.</remarks>
        private readonly List<IGameStatic> _items = new List<IGameStatic>();

        private State _state = State.NewGame;

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
            switch(_state)
            {
                case State.NewGame:
                    NewGame();
                    _state = State.GamePlay;
                    break;

                case State.GamePlay:
                    GamePlay();
                    break;
            }
        }

        private void NewGame()
        {
            foreach(IGameStatic staticItem in _items)
            {
                staticItem.NewGame();
            }
        }

        private void GamePlay()
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