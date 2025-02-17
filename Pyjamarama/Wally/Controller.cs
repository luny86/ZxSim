
using Builder;
using ZX.Game;
using ZX.Platform;

namespace Pyjamarama.Wally
{
    /// <summary>
    /// Main wally controller
    /// </summary>
    internal class Controller : IGameItem, IBuildable
    {
        private IUserInput _input = null!;

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

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
        }

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest("ZX.Platform.UserInput",
                typeof(ZX.Platform.IUserInput));
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _input = dependencies.TryGetInstance("ZX.Platform.UserInput",
                typeof(ZX.Platform.IUserInput))
                as IUserInput
                ?? throw new InvalidOperationException("Unable to get dependency ZX.Platform.UserInput.");

            _input.InputPressed += InputPressedHandler;
            _input.InputReleased += InputReleasedHandler;
        }

        void IBuildable.EndBuild()
        {
        }

        private void InputPressedHandler(object? sender, UserInputFlags inputFlags)
        {
            Console.WriteLine($"P - {inputFlags}");
        }

        private void InputReleasedHandler(object? sender, UserInputFlags inputFlags)
        {
            Console.WriteLine($"R - {inputFlags}");            
        }
    }
}