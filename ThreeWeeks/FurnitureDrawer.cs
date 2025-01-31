using ZX.Util;
using System.Collections.Generic;

namespace ThreeWeeks
{
    public class FurnitureDrawer : KUi.FurnitureDrawer
    {
        #region Constructor
        public FurnitureDrawer(Chunk tileStrings, 
            Chunk stringTable,
            IChunk tileChunk)
            : base (tileChunk,
                    new FurnitureContainer(stringTable, tileStrings))
        {
            CreateCodeMethods();
        }

        private void CreateCodeMethods()
        {
            CodeMethods = new Dictionary<byte, CodeMethod>();
            CodeMethods.Add((byte)FurnitureCode.DrawTile, DrawTile);
            CodeMethods.Add((byte)FurnitureCode.SetCursor, SetCursor);
            CodeMethods.Add((byte)FurnitureCode.SetAddress, SetAddress);
            CodeMethods.Add((byte)FurnitureCode.DrawTileLoop, DrawTileLoop);
            CodeMethods.Add((byte)FurnitureCode.CursorLeftDown, CursorLeftDown);
            CodeMethods.Add((byte)FurnitureCode.DrawSolidTile, DrawSolidTile);
            CodeMethods.Add((byte)FurnitureCode.DrawTileLoopStepUpLeft, DrawTileLoopStepUpLeft);
            CodeMethods.Add((byte)FurnitureCode.CursorRight, CursorRight);
            CodeMethods.Add((byte)FurnitureCode.ToggleBrightness, ToggleBrightness);
            CodeMethods.Add((byte)FurnitureCode.DrawTwoTilesLoopDown, DrawTwoTilesLoopDown);
            CodeMethods.Add((byte)FurnitureCode.DrawTwoTilesLoopAcross, DrawTwoTilesLoopAcross);
            CodeMethods.Add((byte)FurnitureCode.SetColourInk, SetColourInk);
            CodeMethods.Add((byte)FurnitureCode.SetColourInkBright, SetColourInkBright);
            CodeMethods.Add((byte)FurnitureCode.SwitchString, SwitchString);
            CodeMethods.Add((byte)FurnitureCode.SetColourMem, SetColourMem);
            CodeMethods.Add((byte)FurnitureCode.DrawRectangle, DrawRectangle);
            CodeMethods.Add((byte)FurnitureCode.DrawLoopAndRight, DrawLoopAndRight);
            CodeMethods.Add((byte)FurnitureCode.TestFlag, TestFlag);
            CodeMethods.Add((byte)FurnitureCode.Exit, Exit);
        }
        #endregion

        #region Code Methods
        private void DrawTile(CodeArgs args)
        {
            TileDrawer.Draw(X,Y, args.Args[0], Image);
            CursorRight();
        }

        private void SetCursor(CodeArgs args)
        {
            // Some 8-bit maths so cursor goes in the right direction.
            byte byX = (byte)(args.Args[0] - 0x7d);
            byte byY = args.Args[1];
            sbyte offx = (sbyte)byX;
            sbyte offy = (sbyte)byY;

            X+=offx * CharSize;
            Y+=offy * CharSize;                
        }

        private void SetAddress(CodeArgs args)
        {
            ushort newAddr = Chunk.Word(args.Args[1], args.Args[2]);
            // TODO - Add method to draw to set start...
            TileDrawer.SetTileStart(newAddr);
        }

        private void DrawTileLoop(CodeArgs args)
        {
            int count = args.Args[0] - 0xa0;
            int tile = args.Args[1];

            for(int i=0;i<count;i++)
            {
                TileDrawer.Draw(X, Y, tile, Image);
                CursorDown();
            }
        }

        private void CursorLeftDown(CodeArgs args)
        {
            CursorLeft();
            CursorDown();
        }

        private void DrawSolidTile(CodeArgs args)
        {
            int tile = TileDrawer.TileFromAddr(0x4335);
            TileDrawer.Draw(X, Y, tile, Image);
            CursorRight();
        }

        private void DrawTileLoopStepUpLeft(CodeArgs args)
        {
            int count = args.Args[1];
            int tile = args.Args[2];

            for(int i=0; i<count; i++)
            {
                TileDrawer.Draw(X, Y, tile, Image);
                CursorUp();
                CursorRight();
            }
        }

        private void CursorRight(CodeArgs args)
        {
            CursorRight();
        }

        private void ToggleBrightness(CodeArgs args)
        {
            ZX.Palette.ToggleBright(TileDrawer);
        }

        private void DrawTwoTilesLoopDown(CodeArgs args)
        {
            DrawTwoTilesLoopAndMove(args, CursorDown);
        }

        private void DrawTwoTilesLoopAcross(CodeArgs args)
        {
            DrawTwoTilesLoopAndMove(args, CursorRight);
        }

        private void DrawTwoTilesLoopAndMove(CodeArgs args, CursorMethod method)
        {
            int amount = args.Args[1];
            int tile1 = args.Args[2];
            int tile2 = args.Args[3];

            for(int i=0; i<amount; i++)
            {
                TileDrawer.Draw(X, Y, tile1, Image);
                method.Invoke();
                TileDrawer.Draw(X, Y, tile2, Image);
                method.Invoke();
            }
        }

        private void SetColourInk(CodeArgs args)
        {
            int c = args.Args[0] - 0xc2;
            ZX.Palette.SetAttribute((byte)c, TileDrawer);
        }

        private void SetColourInkBright(CodeArgs args)
        {
            int c = args.Args[0] - 0x89;
            ZX.Palette.SetAttribute((byte)c, TileDrawer);
        }

        private void SwitchString(CodeArgs args)
        {
            // Does nothing, handled by enumerator methods.
        }

        private void SetColourMem(CodeArgs args)
        {
            byte c = args.Args[1];
            ZX.Palette.SetAttribute(c, TileDrawer);
        }

        private void DrawRectangle(CodeArgs args)
        {
            int width = args.Args[1];
            int height = args.Args[2];
            int tile = args.Args[3];

            for(int y=0; y<height; y++)
            {
                for(int x=0; x<width;x++)
                {
                    TileDrawer.Draw(
                        X+(x*CharSize),
                        Y+(y*CharSize), 
                        tile,
                        Image);
                }
            }
        }

        private void TestFlag(CodeArgs args)
        {
        }

        private void DrawLoopAndRight(CodeArgs args)
        {
            int count = args.Args[1];
            int tile = args.Args[2];
            for(int i=0; i< count; i++)
            {
                TileDrawer.Draw(X, Y, tile, Image);
                CursorRight();
            }
        }

        private void Exit(CodeArgs args)
        {
        }
        #endregion

    }
}