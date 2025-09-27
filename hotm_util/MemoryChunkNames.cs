
namespace HOTM;

static internal class MemoryChunkNames
{
    /// <summary>
    /// Name of main memory map object
    /// </summary>
    public static string MemoryMap = "HOTM.MemoryMap";

    /// <summary>
    /// List of words in size order.
    /// </summary>
    public static string Dictionary = "HOTM.Dictionary";

    /// <summary>
    /// Strings made up of word tokens, refering to dictionary.
    /// </summary>
    public static string WordTokenTableA = "HOTM.WordTokenTableA";
    public static string WordTokenTableDescriptions = "HOTM.WordTokenTableB";

    /// <summary>
    /// Word decoder using table A and main dictionary
    /// </summary>
    public static string WordDecoder = "HOTM.WordDecoder";

    public static string ObjectData = "HOTM.ObjectData";

    /// <summary>
    /// Raw bitmap data.
    /// </summary>
    public static string Bitmaps = "HOTM.Bitmaps";

    /// <summary>
    /// Information on each graphic holding
    /// size and address of image.
    /// </summary>
    public static string Sprites = "HOTM.Sprites";

    /// <summary>
    /// Basic room data.
    /// </summary>
    public static string Rooms = "HOTM.Rooms";

    public static string ResponseParser = "HOTM.Responses";
}