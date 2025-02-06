using NUnit.Framework;
using System.Text.Json;
using GameEditorLib.Project;

namespace Tests.Builder
{
    public class CunkSerializerTests
    {
        static string ChunkWithAllAttributes = 
            "  { \"Name\" : \"Albert\", \"Start\" : 32768, \"Length\" : 4912 } ";

        static string ChunkWithNoName =
            "{ \"Start\" : 32768, \"Length\" : 4912 }";

        static string ChunkWithNoStart = 
            "  { \"Name\" : \"Albert\", \"Length\" : 4912 } ";

        static string ChunkWithNoLength = 
            "  { \"Name\" : \"Albert\", \"Start\" : 4912 } ";


        [Test]
        public void BasicSerializeTest()
        {
            ChunkSerializer? chunk = 
                JsonSerializer.Deserialize<ChunkSerializer>(ChunkWithAllAttributes);

            Assert.NotNull(chunk);
            Assert.That(chunk.Name, Is.EqualTo("Albert"));
            Assert.That(chunk.Start, Is.EqualTo(32768));
            Assert.That(chunk.Length, Is.EqualTo(4912));
        }

        [Test]
        public void DeserializeChunkWithNoName()
        {
            RunTestForMissingRequiredProperty(ChunkWithNoName);
        }

        [Test]
        public void DeserializeChunkWithNoStart()
        {
            RunTestForMissingRequiredProperty(ChunkWithNoStart);
        }

        [Test]
        public void DeserializeChunkWithNoLength()
        {
            RunTestForMissingRequiredProperty(ChunkWithNoLength);
        }

        private void RunTestForMissingRequiredProperty(string testString)
        {
            ChunkSerializer? chunk = null;

            Assert.Throws(typeof(JsonException), 
                () => JsonSerializer.Deserialize<ChunkSerializer>(testString),
                "Name is missing, this should be required, but has been allowed.");
        }
    }
}

