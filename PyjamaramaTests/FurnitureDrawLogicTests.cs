
using ZX.Platform;
using ZX.Util;
using ZX.Drawing;
using Pyjamarama;

namespace PyjamaramaTests
{
    public class FurnitureDrawLogicTests
    {
        // Empty test mock class.
        private class Surface : ISurface
        {
            event EventHandler ISurface.Updated
            {
                add {}
                remove {}
            }

            void ISurface.BeginDraw() {}
            void ISurface.Blend(ISurface to, int x, int y) {}
            void ISurface.Create(int w, int h) {}
            void ISurface.EndDraw() {}
            void ISurface.Fill(Rgba colour) {}
            void ISurface.FillRect(Rectangle rect, Rgba colour) {}
            bool ISurface.IsInBounds(int x, int y) { return true; }
            void ISurface.SetPixel(int x, int y, Rgba colour) {}
        }

        // Empty mock class.
        private class Drawer : IDrawer
        {
            void IDrawer.Draw(ISurface surface, int index, int x, int y)
            {
            }
        }

        private FurnitureDrawLogic CreateBasicLogic(byte[] commandString)
        {
            return new FurnitureDrawLogic()
            {
                Surface = new Surface(),
                TileDrawer = new Drawer(),
                Data = new Chunk("T1", 0x4000, commandString.Length, commandString),
                X = 0,
                Y = 0,
                Index = 0
            };
        }
        [Test]
        public void DrawTileAtCurrentPositionTest()
        {
            const int moveX = 1;
            const int moveY = 0;
            const int dataSize = 1;

            FurnitureDrawLogic logic = CreateBasicLogic([0]);

            logic.DrawTileAtCurrentPosition(0);
            Assert.That(logic.X, Is.EqualTo(moveX), "X position was not updated as expected.");
            Assert.That(logic.Y, Is.EqualTo(moveY), "Y position was not updated as expected.");
            Assert.That(logic.Index, Is.EqualTo(dataSize), "Index was not updated as expected.");
        }
 
         [Test]
        public void SetPositionCommandTest()
        {
            const int moveX = 7;
            const int moveY = 4;
            const int dataSize = 3;

            FurnitureDrawLogic logic = CreateBasicLogic([0, moveX, moveY]);

            logic.SetPositionCommand();
            Assert.That(logic.X, Is.EqualTo(moveX), "X position was not updated as expected.");
            Assert.That(logic.Y, Is.EqualTo(moveY), "Y position was not updated as expected.");
            Assert.That(logic.Index, Is.EqualTo(dataSize), "Index was not updated as expected.");
        }

        [Test]
        public void SetOriginCommandTest()
        {
            const int offset = 0x8834 / 8;
            FurnitureDrawLogic logic = CreateBasicLogic([0,0x34, 0x98]);

            logic.SetOriginCommand(0x1000);

            Assert.That(logic.Offset, Is.EqualTo(offset), "Origin not set corectly based on data.");
            Assert.That(logic.Index, Is.EqualTo(3), "Index was not updated as expected.");
        }

        [Test]
        public void DrawRepeatedTileCommandTest()
        {
            FurnitureDrawLogic logic = CreateBasicLogic([0, 8, 1]);

            logic.DrawRepeatedTileCommand();

            Assert.That(logic.X, Is.EqualTo(8), "X position was not updated as expected.");
            Assert.That(logic.Y, Is.EqualTo(0), "Y position was not updated as expected.");
            Assert.That(logic.Index, Is.EqualTo(3), "Index was not updated as expected.");
        }

        [Test]
        public void CurrentCodeTest()
        {
            FurnitureDrawLogic logic = CreateBasicLogic([1,2,3,4]);
            logic.Index = 2;

            Assert.That(logic.CurrentCode, Is.EqualTo(3));

        }

        [Test]
        public void CurrentAsTileIndexTest()
        {
            FurnitureDrawLogic logic = CreateBasicLogic([5]);
            logic.Offset = 8;

            Assert.That(logic.CurrentAsTileIndex, Is.EqualTo(5+8));
        }

        [Test]
        public void GetAttributeCommandTest()
        {
            const byte expectedAttr = 0x45;
            FurnitureDrawLogic logic = CreateBasicLogic([0,expectedAttr]);
            
            Assert.That(logic.GetAttributeCommand(), Is.EqualTo(expectedAttr));
            Assert.That(logic.Index, Is.EqualTo(2));
        }
   }

}