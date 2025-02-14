
using ZX.Game;

namespace Pyjamarama.Wally
{
    /// <summary>
    /// Main wally controller
    /// </summary>
    internal class Controller : IGameItem
    {
        public DrawLayer Layer
        {
            get;
            init;
        } = null!;

        void IGameStatic.NewGame()
        {
        }

        void IGameStatic.NewLevel()
        {
        }

        void IGameItem.Update()
        {
            if(++Layer.Frame > 6)
            {
                Layer.Frame = 0;
            }
            
            Layer?.Update();
        }
    }
}