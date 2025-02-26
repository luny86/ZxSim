
using System.Drawing;

namespace Pyjamarama.Wally
{
    /// <summary>
    /// Logic for handling Wally's jumping.
    /// </summary>
    internal class JumpHandler
    {
        private struct Vector
        {
            public int x;
            public int y;
            public int imgIndex;

            public Vector(int x, int y, int imgIndex)
            {
                this.x = x;
                this.y = y;
                this.imgIndex = imgIndex;
            }
        }

        // Table of vectors and image index used for each
        // stage of the jump.
        private Vector[] vectors = new Vector[]
        {
            new Vector(2, -4, 0x00),
            new Vector(2, -4, 0x02 ),
            new Vector(2, -4, 0x04 ),
            new Vector(2, -4, 0x04 ),
            new Vector(2, -4, 0x06 ),
            new Vector(2, -4, 0x06 ),
            new Vector(2,  4, 0x08 ),
            new Vector(2,  4, 0x08 ),
            new Vector(2,  4, 0x0a ),
            new Vector(2,  4, 0x0a ),
            new Vector(2,  4, 0x0c ),
            new Vector(2,  4, 0x0e )
        };

        public JumpHandler()
        {
        }

        public bool IsJumping
        {
            get
            {
                return (Direction != DirType.None);
            }
        }

        public DirType Direction
        {
            get;
            private set;
        }

        public int Count
        {
            get;
            private set;
        }

        public void Initialise(DirType direction)
        {
            Direction = direction;
            Count = 0;
        }

        public void Reset()
        {
            Direction = DirType.None;
            Count = 0;
        }

        /// <summary>
        /// Update jump if in progress.
        /// </summary>
        /// <param name="wally">Wally.</param>
        /// <returns>true if jump has finished.</returns>
        public bool Update(Controller wally)
        {
            bool finished = false;

            wally.HeadTurned = false;

            if (Direction == DirType.Left)
            {
                bool stopMove = (vectors[Count].y > 0 && wally.IsOnPlatform);

                // $acb1 - Falling
                if (!stopMove)
                {
                    Point position = wally.Position;
                    position.Y += vectors[Count].y;
                    position.X -= vectors[Count].x;
                    if (position.X < 0x07)
                    {
                        position.X = 0x07;
                    }

                    wally.Position = position;
                }

                wally.Frame = vectors[Count].imgIndex + 0x20;
            }
            else
            {
                // $ad12
                if ((vectors[Count].y > 0 && !wally.IsOnPlatform) || vectors[Count].y <= 0)
                {
                    Point position = wally.Position;
                
                    position.Y += vectors[Count].y;
                    position.X += vectors[Count].x;
                    if (position.X > 0xe9)
                    {
                        position.X = 0xe9;
                    }

                    wally.Position = position;
                }

                wally.Frame = vectors[Count].imgIndex + 0x30;
            }

            if (++Count >= 0x0c)
            {
                wally.Direction = DirType.None;
                Direction = DirType.None;
                finished = true;
            }
            
            return finished;
        }
    }
}