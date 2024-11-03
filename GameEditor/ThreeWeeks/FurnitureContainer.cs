
using System.Collections;
using System.Collections.Generic;
using KUtil;

namespace ThreeWeeks
{
    internal class FurnitureContainer : IDataContainer
    {
        #region Private Types
        private class FurnitureEnumerable : IEnumerable
        {
            public FurnitureEnumerable(int startOffset, 
                IChunk furnitureData,
                IReadOnlyDictionary<byte, CodeInfo> info,
                IReadOnlyList<Range> ranges)
            {
                StartOffset = startOffset;
                FurnitureData = furnitureData;
                CodeInfo = info;
                Ranges = ranges;
            }

            private int StartOffset { get; }
            private IChunk FurnitureData { get; }
            private IReadOnlyDictionary<byte, CodeInfo> CodeInfo { get; }
            private IReadOnlyList<Range> Ranges { get; }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new FurnitureEnumerator(StartOffset, FurnitureData, CodeInfo, Ranges);
            }
        }
        #endregion

        #region Construction
        public FurnitureContainer(IChunk itemTable, IChunk itemData)
        {
            ItemTable = itemTable;
            ItemData = itemData;
            CreateInfo();
            Ranges = CreateRanges();
        }

        private void CreateInfo()
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

        private List<Range> CreateRanges()
        {
            return new List<Range>()
            {
                new Range(0, (byte)FurnitureCode.DrawTile),
                new Range((byte)FurnitureCode.DrawTile, (byte)FurnitureCode.SetCursor),
                new Range((byte)FurnitureCode.SetCursor, (byte)FurnitureCode.DrawTileLoop),
                new Range((byte)FurnitureCode.DrawTileLoop, (byte)FurnitureCode.SetColourInk),
                new Range((byte)FurnitureCode.SetColourInk, (byte)FurnitureCode.SetColourInkBright),
                new Range((byte)FurnitureCode.SoftEnd, (byte)FurnitureCode.Exit)
            };
        }
        #endregion

        #region Fields
        private IChunk ItemTable { get; }
        private IChunk ItemData { get; }
        private IReadOnlyDictionary<byte, CodeInfo> CodeInfo { get; set; }
        private List<Range> Ranges { get; }
        #endregion

        #region Public API
        IEnumerable IDataContainer.this[int index]
        {
            get
            {
                int offset = CalculateRoomAddressOffset(index);

                return new FurnitureEnumerable(
                    offset,
                    ItemData,
                    CodeInfo,
                    Ranges);
            }
        }

        private int CalculateRoomAddressOffset(int index)
        {
            int word = ItemTable.Word(index*2);
            return  word - ItemData.Start; 
        }
        #endregion
    }
}