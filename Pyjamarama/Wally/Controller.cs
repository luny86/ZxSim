
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
        #region Private Types

        /// <summary>
        /// Determines which way Wally is facing.
        /// </summary>
        private enum DirType
        {
            None,
            Left,
            Right
        };

        #endregion

        #region Private Members

        private IUserInput _input = null!;

        #endregion

        #region Construction

        public DrawLayer Layer
        {
            get;
            set;
        } = null!;

        #endregion

        #region Properties

        /// <summary>
        /// Copy of the user input flags from the last event.
        /// </summary>
        private UserInputFlags UserInput
        {
            get;
            set;
        }

        /// <summary>
        /// Current direction of Wally.
        /// </summary>
        private DirType Direction
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if Wally's head is turned towards the camera.
        /// </summary>
        /// <value><c>true</c> if head turned; otherwise, <c>false</c>.</value>
        private bool HeadTurned
        {
            get;
            set;
        }


        /// <summary>
        /// Last direction when moved.
        /// </summary>
        private DirType LastDir
        {
            get;
            set;
        }
        #endregion

        void IGameStatic.NewGame()
        {
            UserInput = UserInputFlags.None;
            Direction = DirType.None;
        }

        void IGameStatic.NewLevel()
        {
        }

        #region Movement

        void IGameItem.Update()
        {
            CheckForMovement();
            UpdateFrameAndPosition();
            Layer?.Update();
        }

        private void CheckForMovement()
        {
            UserInputFlags input = UserInput;

            if((input & UserInputFlags.Left) == UserInputFlags.Left)
            {
                Direction = DirType.Left;
            }
            else if ((input & UserInputFlags.Right) == UserInputFlags.Right)
            {
                Direction = DirType.Right;
            }
            else
            {
                Direction = DirType.None;
            }
        }

        private void UpdateFrameAndPosition()
        {
            int x = Layer.X;
            int y = Layer.Y;
            int frame = Layer.Frame;

            // Move Left
            if (Direction == DirType.Left)
            {
                //HeadTurn();

                if (frame >= 0x08)
                {
                    frame = 0x00;
                    HeadTurned = false;
                }
                else
                {
                    frame += 2;
                    if (frame >= 0x08)
                    {
                        frame = 0;
                        HeadTurned = false;
                    }
                }

                x -= 2;
                if (x < 0x07)
                {
                    x = 7;
                }

                LastDir = Direction;
            }
            else if (Direction == DirType.Right)
            {
                //HeadTurn();
                if (frame < 0x08)
                {
                    frame = 0x10;
                    HeadTurned = false;
                }
                else
                {
                    frame += 2;
                    if (frame >= 0x10)
                    {
                        frame = 0x08;
                        HeadTurned = false;
                    }
                }

                x += 2;
                if (x >= 0xe9)
                {
                    x = 0xe9;
                }

                LastDir = Direction;
            }

            Layer.Frame = frame;
            Layer.X = x;
            Layer.Y = y;
        }

        #endregion

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
            UserInput = inputFlags;
        }

        private void InputReleasedHandler(object? sender, UserInputFlags inputFlags)
        {
            UserInput = inputFlags;
        }
    }
}