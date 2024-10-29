
namespace ThreeWeeks
{
    /// <summary>
    /// Codes used by the furniture draw decoder.
    /// </summary>
    enum FurnitureCode : byte
    {
        DrawTile = 0x5c,    // Less then is a tile
        SetCursor = 0x9e,   
        DrawTileLoop = 0xc1,
        SetColourInk = 0xca,
        SetColourInkBright = 0xd1,
        ToggleBrightness = 0xf1,
        DrawSolidTile = 0xf2,
        CursorLeftDown = 0xf3,
        SetColourMem = 0xf4,
        CursorRight = 0xf5,
        SwitchString = 0xf6,
        DrawTwoTilesLoopAcross = 0xf7,
        DrawTwoTilesLoopDown = 0xf8,
        DrawTileLoopStepUpLeft = 0xf9,
        DrawRectangle = 0xfa,
        SetAddress = 0xfb,
        TestFlag = 0xfc,
        DrawLoopAndRight = 0xfd,
        SoftEnd = 0xfe,
        Exit = 0xFF
    }
}