
using System.Text.Json.Serialization;

namespace GameEditorLib.Project
{
    /// <summary>
    /// Serializes the project attributes.
    /// </summary>
    internal class ProjectSerializer
    {
        /// <summary>
        /// Name of the project.
        /// </summary>
        public required string Name { get; set; } = string.Empty;

        /// <summary>
        /// Name of the binary file.
        /// </summary>
        /// <remarks>
        /// Any related binary snapshot should be in the same folder
        /// as the project.
        public required string Binary { get; set; } = string.Empty;
    }
}
