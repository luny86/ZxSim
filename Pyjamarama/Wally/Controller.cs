
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
    internal class Controller : IGameItem, IBuildable, IPlayer
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

        private const int WallyFrameLeft = 0;
        private const int WallyFrameRight = 0x10;

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
        public bool Disabled
        {
            get;
            private set;
        } = false;

        public bool IsDead
        {
            get;
            private set;
        } = false;

        public bool JustPickedUp
        {
            get;
            set;
        } = false;

        public bool Visible
        {
            get 
            { 
                return Layer?.Visible ?? false; 
            }

            set
            {
                Layer.Visible = value;
            }
        }
        
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
            get { return Layer.HeadTurned; }
            set { Layer.HeadTurned = value; }
        }

        /// <summary>
        /// Gets and sets the countdown to the next head turn.
        /// </summary>
        private int HeadCount
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
            HeadTurned = false;
            Falling = false;
            Direction = DirType.None;
            //Disabled = false;
            //IsDead = false;
            Jump.Reset();

            if (Position.X < 0x78)
            {
                Frame = WallyFrameRight;
                LastDir = DirType.Right;
            }
            else
            {
                Frame = WallyFrameLeft;
                LastDir = DirType.Left;
            }
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


            if ((input & UserInputFlags.FireA) == UserInputFlags.FireA)
            {
                //if (_flags[FlagsNames.ArcadeMode].Value == 0)
                {
                    // Set up jump
                    //Energy.Update();

                    DirType direction = Direction;
                    if (direction == DirType.None)
                    {
                        direction = LastDir;
                    }

                    Jump.Initialise(direction);
                }
            }
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
                HeadTurn();

                if (frame >= WallyFrameRight)
                {
                    frame = WallyFrameLeft;
                    HeadTurned = false;
                }
                else
                {
                    frame += 2;
                    if (frame >= WallyFrameRight)
                    {
                        frame = WallyFrameLeft;
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
                HeadTurn();
                if (frame < WallyFrameRight)
                {
                    frame = WallyFrameRight;
                    HeadTurned = false;
                }
                else
                {
                    frame += 2;
                    if (frame >= 0x20)
                    {
                        frame = WallyFrameRight;
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

        private void HeadTurn()
        {
            Random r = new Random();

            switch (Frame)
            {
                case WallyFrameLeft:
                case WallyFrameRight:
                    if (HeadCount > 0)
                    {
                        HeadCount--;
                    }
                    else
                    {
                        HeadTurned = true;
                    }
                    break;

                case 0x0c:
                case 0x1c:
                    if (HeadCount > 0)
                    {
                        HeadCount--;
                    }
                    else
                    {
                        HeadTurned = false;
                        HeadCount = (r.Next() & 0x1f);
                    }
                    break;

                default:
                    if (HeadCount > 0)
                    {
                        HeadCount--;             
                    }
                    break;
            }
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

        #region IBuildable

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

        #endregion

        #region Input Handlers
        private void InputPressedHandler(object? sender, UserInputFlags inputFlags)
        {
            UserInput = inputFlags;
        }

        private void InputReleasedHandler(object? sender, UserInputFlags inputFlags)
        {
            UserInput = inputFlags;
        }

        #endregion
    }
}