using KUtil;
using System.Collections.Generic;

namespace ThreeWeeks
{
    internal class FurnitureDrawer : KUi.FurnitureDrawer
    {
        #region Constructor
        public FurnitureDrawer(Chunk tileStrings, 
            Chunk stringTable,
            IChunk tileChunk)
            : base (tileStrings, stringTable, tileChunk)
        {
            CreateRanges();
            CreateCodeInfo();
            CreateCodeMethods();
        }

        private void CreateRanges()
        {
            Ranges = new List<Range>()
            {
                new Range(0, (byte)FurnitureCode.DrawTile),
                new Range((byte)FurnitureCode.DrawTile, (byte)FurnitureCode.SetCursor),
                new Range((byte)FurnitureCode.SetCursor, (byte)FurnitureCode.DrawTileLoop),
                new Range((byte)FurnitureCode.DrawTileLoop, (byte)FurnitureCode.SetColourInk),
                new Range((byte)FurnitureCode.SetColourInk, (byte)FurnitureCode.SetColourInkBright),
                new Range((byte)FurnitureCode.SoftEnd, (byte)FurnitureCode.Exit)
            };
        }

        private void CreateCodeInfo()
        {
            CodeInfo = new Dictionary<byte, CodeInfo>()
            {
                { (byte)FurnitureCode.DrawTile, new CodeInfo((byte)FurnitureCode.DrawTile, 1, "Draw Tile") },
                { (byte)FurnitureCode.SetCursor, new CodeInfo((byte)FurnitureCode.SetCursor, 2, "Set Cursor") },
                { (byte)FurnitureCode.SetAddress, new CodeInfo((byte)FurnitureCode.SetAddress, 3, "Set Address") },
                { (byte)FurnitureCode.DrawTileLoop, new CodeInfo((byte)FurnitureCode.DrawTileLoop, 2, "Draw Tile Loop") },
                { (byte)FurnitureCode.CursorLeftDown, new CodeInfo((byte)FurnitureCode.CursorLeftDown, 1, "Cursor left/down") },
                { (byte)FurnitureCode.DrawSolidTile, new CodeInfo((byte)FurnitureCode.DrawSolidTile, 1, "Draw Solid Tile") },
                { (byte)FurnitureCode.DrawTileLoopStepUpLeft, new CodeInfo((byte)FurnitureCode.DrawTileLoopStepUpLeft, 3, "Draw Loop, Step up-left") },
                { (byte)FurnitureCode.CursorRight, new CodeInfo((byte)FurnitureCode.CursorRight, 1, "Cursor Right") },
                { (byte)FurnitureCode.ToggleBrightness, new CodeInfo((byte)FurnitureCode.ToggleBrightness, 1, "Toggle Bright") },
                { (byte)FurnitureCode.DrawTwoTilesLoopDown, new CodeInfo((byte)FurnitureCode.DrawTwoTilesLoopDown, 4, "Draw Two Tiles Loop Down") },
                { (byte)FurnitureCode.DrawTwoTilesLoopAcross, new CodeInfo((byte)FurnitureCode.DrawTwoTilesLoopAcross, 4, "Draw Two Tiles Loop Across") },
                { (byte)FurnitureCode.SetColourInk, new CodeInfo((byte)FurnitureCode.SetColourInk, 1, "Set Colour Ink") },
                { (byte)FurnitureCode.SetColourInkBright, new CodeInfo((byte)FurnitureCode.SetColourInkBright, 1, "Set Colour Ink Bright") },
                { (byte)FurnitureCode.SwitchString, new CodeInfo((byte)FurnitureCode.SwitchString, 1, "Switch String") },
                { (byte)FurnitureCode.SetColourMem, new CodeInfo((byte)FurnitureCode.SetColourMem, 2, "Set Attribute N") }, 
                { (byte)FurnitureCode.DrawRectangle, new CodeInfo((byte)FurnitureCode.DrawRectangle, 4, "Draw Rectangle") },
                { (byte)FurnitureCode.TestFlag, new CodeInfo((byte)FurnitureCode.TestFlag, 1, "Test Flag") },
                { (byte)FurnitureCode.DrawLoopAndRight, new CodeInfo((byte)FurnitureCode.DrawLoopAndRight, 3, "Draw Loop And Right") },
                { (byte)FurnitureCode.Exit, new CodeInfo((byte)FurnitureCode.Exit, 1, "Exit") }
            };        
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
            zx.Palette.ToggleBright(TileDrawer);
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
            zx.Palette.SetAttribute((byte)c, TileDrawer);
        }

        private void SetColourInkBright(CodeArgs args)
        {
            int c = args.Args[0] - 0x89;
            zx.Palette.SetAttribute((byte)c, TileDrawer);
        }

        private void SwitchString(CodeArgs args)
        {
            Offset = TileStringChunk.Word(Offset) - TileStringChunk.Start;
        }

        private void SetColourMem(CodeArgs args)
        {
            byte c = args.Args[1];
            zx.Palette.SetAttribute(c, TileDrawer);
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
            Offset++;   // TODO Ignore flag bit for now...
            if(Flag)
            {
                // Skip original ending for alternate ending.
                while(TileStringChunk[Offset++] != (byte)FurnitureCode.SoftEnd);
            }
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
            Offset = -1;
        }
        #endregion

    }
}