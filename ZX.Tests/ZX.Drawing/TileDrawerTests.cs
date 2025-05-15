using System.Text;
using ZX.Util;
using ZX.Platform;
using ZX.Drawing;

namespace ZX.Tests.ZX.Drawing
{
    public class TileDrawerTests
    {
        /// <summary>
        /// Tile bitmap as would be stored in a spectrum game.
        /// </summary>
        static readonly byte[] _tileBinary =
                [
                    0xff, 0x00, 0x81, 0x7e, 0x00,0x00,0x81,0xff
                ];

        /// <summary>
        /// Final result of the tile draw based on
        /// how the test drawer to do it.
        /// </summary>
        static readonly byte[] _expectedPattern =
                [
                    1,1,1,1,1,1,1,1,
                    0,0,0,0,0,0,0,0,
                    1,0,0,0,0,0,0,1,
                    0,1,1,1,1,1,1,0,
                    0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,
                    1,0,0,0,0,0,0,1,
                    1,1,1,1,1,1,1,1
                ];


        /// <summary>
        /// Test memory chunk that gives the tile pattern
        /// to represent the Spectrum memory.
        /// </summary>
        private class Pattern : IChunk
        {
            private byte[] _tile;

            public Pattern()
            {
                _tile = _tileBinary;

            }

            public byte this[int index]
            {
                get
                {
                    return _tile[index];
                }

                set
                {
                    //
                }
            }

            public byte[] Tile => _tile;

            ushort IReadOnlyChunk.Word(int index)
            {
                return 0;
            }

            byte[] IReadOnlyChunk.CopyRange(int index, int length)
            {
                return _tile;
            }

            string IChunkAttributes.Name => "Name";

            int IChunkAttributes.Start => 0;

            int IChunkAttributes.Length => 64;


            public bool IsInRange(int address)
            {
                return (address >= (this as IChunkAttributes).Start &&
                        address < (this as IChunkAttributes).Start + (this as IChunkAttributes).Length);
            }
        }

        /// <summary>
        /// Test surface for capturing the tile draw results.
        /// </summary>
        private class Surface : ISurface
        {
            private byte[] _pattern = new byte[64];

            event EventHandler ISurface.Updated
            {
                add
                {
                    throw new NotImplementedException();
                }

                remove
                {
                    throw new NotImplementedException();
                }
            }

            void ISurface.BeginDraw()
            {
                throw new NotImplementedException();
            }

            void ISurface.Blend(ISurface to, int x, int y)
            {
                throw new NotImplementedException();
            }

            void ISurface.Create(int w, int h)
            {
            }

            void ISurface.EndDraw()
            {
            }

            void ISurface.Fill(Rgba colour)
            {
            }

            void ISurface.FillRect(Rectangle rect, Rgba colour)
            {
            }

            bool ISurface.IsInBounds(int x, int y)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Instead of drawing to an image we capture
            /// each pixel as data.
            /// </summary>
            void ISurface.SetPixel(int x, int y, Rgba colour)
            {
                byte value = (byte)
                    (colour.A == 1 && colour.R == 0 && colour.G == 0 && colour.B == 0 ?
                    0 : 1);

                _pattern[x + (y * 8)] = value;
            }

            /// <summary>
            /// Run a match and see if the given pattern matches
            /// what was capture here.
            /// </summary>
            public bool IsPattern(byte[] pattern)
            {
                bool same = true;

                for (int i = 0; i < _pattern.Length; i++)
                {
                    if (_pattern[i] != pattern[i])
                    {
                        same = false;
                        break;
                    }
                }

                return same;
            }
        }


        [Test]
        public void DrawTileTest()
        {
            Pattern tile = new Pattern();
            TileDrawer drawer = new TileDrawer(tile);
            // Make sure things are drawn with specific RGB values.
            (drawer as IAttribute).Paper = new Rgba(0, 0, 0, 1);
            (drawer as IAttribute).Ink = new Rgba(1, 1, 1, 1);

            Surface surface = new Surface();

            drawer.Draw(surface, 0, 0, 0);

            Assert.True(surface.IsPattern(_expectedPattern), $"Tile draw pattern does not match source.");
        }
    }
}