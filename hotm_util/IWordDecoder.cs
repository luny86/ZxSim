namespace HOTM;

/// <summary>
/// Describes a HOTM Word Decoder.
/// </summary>
public interface IWordDecoder
{
    /// <summary>
    /// Create a string from a word token string.
    /// </summary>
    /// <param name="index">Index of word token string.</param>
    /// <returns>String of words.</returns>
    string DecodeString(int index);

    /// <summary>
    /// Get a word from the dictionary.
    /// </summary>
    /// <param name="size">Word size</param>
    /// <param name="index">Word index</param>
    /// <returns>Word as a string.</returns>
    string GetWord(int size, int index);
}