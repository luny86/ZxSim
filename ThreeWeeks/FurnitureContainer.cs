
using System.Collections;
using System.Collections.Generic;
using ZX.Util;

namespace ThreeWeeks
{
    internal class FurnitureContainer : IDataContainer
    {
        #region Private Types
        private class FurnitureEnumerable : IEnumerable
        {
            public FurnitureEnumerable(int startOffset, 
                FurnitureDataGroup furnitureDataGroup)
            {
                StartOffset = startOffset;
                dataGroup = furnitureDataGroup;
            }

            FurnitureDataGroup dataGroup { get; } 

            private int StartOffset { get; }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new FurnitureEnumerator(StartOffset, dataGroup);
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
            EnumeratorMethods = CreateEnumeratorMap();
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

        private List<KUtil.Range> CreateRanges()
        {
            return new List<KUtil.Range>()
            {
                new KUtil.Range(0, (byte)FurnitureCode.DrawTile),
                new KUtil.Range((byte)FurnitureCode.DrawTile, (byte)FurnitureCode.SetCursor),
                new KUtil.Range((byte)FurnitureCode.SetCursor, (byte)FurnitureCode.DrawTileLoop),
                new KUtil.Range((byte)FurnitureCode.DrawTileLoop, (byte)FurnitureCode.SetColourInk),
                new KUtil.Range((byte)FurnitureCode.SetColourInk, (byte)FurnitureCode.SetColourInkBright),
                new KUtil.Range((byte)FurnitureCode.SoftEnd, (byte)FurnitureCode.Exit)
            };
        }
        #endregion

        #region Enumerator altering methods and map
        private IReadOnlyDictionary<byte, EnumeratorAlterMethod> CreateEnumeratorMap()
        {
            return new  Dictionary<byte, EnumeratorAlterMethod>()
            {
                { (byte)FurnitureCode.SwitchString, SwitchString },
                { (byte)FurnitureCode.TestFlag, TestFlag }
            };
        }

        int SwitchString(int index, FurnitureDataGroup group)
        {
            return group.FurnitureData.Word(index);
        }

        int TestFlag(int index, FurnitureDataGroup group)
        {
            index++; // Skip flag for now
            while(group.FurnitureData[index++] != (byte)FurnitureCode.SoftEnd);
            return index;
        }
        #endregion

        #region Fields
        private IChunk ItemTable { get; }
        private IChunk ItemData { get; }
        private IReadOnlyDictionary<byte, CodeInfo> CodeInfo { get; set; } = null!  ;
        private List<KUtil.Range> Ranges { get; }

        private IReadOnlyDictionary<byte, EnumeratorAlterMethod> EnumeratorMethods { get; }
        #endregion

        #region Public API
        IEnumerable IDataContainer.this[int index]
        {
            get
            {
                int offset = CalculateRoomAddressOffset(index);

                FurnitureDataGroup group = new FurnitureDataGroup()
                {
                    FurnitureData = ItemData,
                    CodeInfoMapping = CodeInfo,
                    CodeRanges = Ranges,
                    SpecialCodes = EnumeratorMethods
                };

                return new FurnitureEnumerable(offset, group);
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