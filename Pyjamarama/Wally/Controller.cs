
using System.Drawing;
using Builder;
using ZX;
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
        /// Different states during the death scenes.
        /// </summary>
        private enum DeathScene
        {
            None,
            Falling,
            SittingDown,
            OnTheDeck,
            LyingDown,
            Resurrect
        }

        #endregion

        #region Private Members

        private IUserInput _input = null!;
        private readonly IAttributeTable _attributeTable;

        #endregion

        #region Construction

        public Controller(IAttributeTable attributeTable)
        {
            _attributeTable = attributeTable;
            Jump = new JumpHandler();
        }
        #endregion

        #region Properties

        public bool Visible
        {
            get;
            set;
        } = true;

        public DrawLayer Layer
        {
            get;
            set;
        } = null!;

        /// <summary>
        /// Copy of the user input flags from the last event.
        /// </summary>
        private UserInputFlags UserInput
        {
            get;
            set;
        }

        public Point Position
        {
            get
            {
                return new Point(Layer.X, Layer.Y);
            }

            set
            {
                Layer.X = value.X;
                Layer.Y = value.Y;
            }
        }

        /// <summary>
        /// Current direction of Wally.
        /// </summary>
        public DirType Direction
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if Wally's head is turned towards the camera.
        /// </summary>
        /// <value><c>true</c> if head turned; otherwise, <c>false</c>.</value>
        public bool HeadTurned
        {
            get;
            set;
        }


        private bool Falling
        {
            get;
            set;
        }

        public int Frame
        {
            get { return Layer.Frame; }
            set { Layer.Frame = value; }
        }

        public bool IsOnPlatform
        {
            get
            {
                bool isOn = false;

                if (IsPlatform(LastAttr))
                {
                    isOn = (Position.Y & 0x07) == 0x00;
                }

                return isOn;
            }
        }

        /// <summary>
        /// Last direction when moved.
        /// </summary>
        private DirType LastDir
        {
            get;
            set;
        }

        private JumpHandler Jump
        {
            get;
            set;
        }

        #endregion

        void IGameStatic.NewGame()
        {
            UserInput = UserInputFlags.None;
            Direction = DirType.None;

            //_attributeTable.Clear(0x46);
        }

        void IGameStatic.NewLevel()
        {

        }

        #region Movement

        void IGameItem.Update()
        {
            StandardUpdate();
            Layer?.Update();
        }


        private void StandardUpdate()
        {
            if (Visible)
            {
                CheckAttr();

                if (!Jump.IsJumping)
                {
                    CheckForFall();
                }
                else
                {
                    HeadTurned = false;
                    // Handle jump in progress.
                    Jump.Update(this);

                    return;
                }


                if (!Jump.IsJumping && !Falling)
                {
                    CheckForMovement();
                    UpdateFrameAndPosition();
                }
            }
        }

        private void CheckForFall()
        {
            // ATTR under feet.
            // See code $ab98
            bool hasPlatform = IsPlatform(LastAttr);

            if (!Falling && !hasPlatform)
            {
                Falling = true;
            }

            if (Falling)
            {
                // Check Falling ($AC4A)
                if (hasPlatform)
                {
                    if ((Position.Y & 0x07) == 0)
                    {
                        Falling = false;
                    }
                }

                if (Falling)
                {
                    Position = new Point(Position.X, Position.Y + 4);
                }
            }
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

        #region Attribute Checks

        private byte LastAttr
        {
            get;
            set;
        }

        private void CheckAttr()
        {
            if (CharAligned)
            {
                // check Wally ATTR
                // See code $ab82
                Point p = new Point(
                    (Position.X + 0x08) / 8,
                    (Position.Y + 0x20) / 8);

                LastAttr = _attributeTable.GetAt(p);
            }
            else
            {
                // $ab9b - Get colour from previous store.
                LastAttr = _attributeTable.GetAt(
                    new Point(
                        (Position.X / 8) + 1,
                        (Position.Y / 8) + 4));
            }
        }

        private bool CharAligned
        {
            get
            {
                return (Layer.Y & 0x07) == 0;
            }
        }

        /// <summary>
        /// Determines whether the given colours represent a platform or not.
        /// </summary>
        /// <param name="colours">Colours.</param>
        /// <returns>
        /// <c>true</c> if this colour is a platform.; 
        /// otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsPlatform(byte colours)
        {
            return (colours == 0x42 || colours == 0x45);
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