
using KUtil;
using System.Text.Json.Serialization;

namespace GameEditorLib.Project
{
    /// <summary>
    /// Serialized object for a memory chunk
    /// </summary>
    internal class ChunkSerializer : IChunkAttributes
    {
        public required string Name { get; set; } = null!;

        public required int Start { get; set; }

        public required int Length { get; set; }
    }
}
