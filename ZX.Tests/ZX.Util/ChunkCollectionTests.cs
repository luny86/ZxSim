using ZX.Util;

namespace ZX.Tests.ZX.Util
{
    public class ChunkCollectionTests
    {
        [Test]
        public void CreatWithName()
        {
            ChunkCollection memory = new ChunkCollection("Name");

            Assert.That(memory.Name, Is.EqualTo("Name"));
        }

        private ChunkCollection CreateMemory()
        {
            // Assume address starts at 0x4000
            byte[] buffer = new byte[0x10000];
            buffer[0x01] = 0xde;
            buffer[0x2023] = 0xad;
            ChunkCollection memory = new ChunkCollection("Chunk");
            memory.Add(new Chunk("1", 0x4000, 0x100, buffer));
            memory.Add(new Chunk("2", 0x6000, 0x100, buffer));

            return memory;
        }

        [Test]
        public void AddressIsInRangeTest()
        {
            ChunkCollection memory = CreateMemory();

            Assert.That(memory[0x4001], Is.EqualTo(0xde), "Address 4001 not found.");
            Assert.That(memory[0x6023] == 0xad, "Address 6023 not found.");
        }

        [Test]
        public void AddressNotInRange()
        {
            ChunkCollection memory = CreateMemory();
            Assert.Throws<IndexOutOfRangeException>(() =>
                { byte data = memory[0x5000]; });
        }
    }
}