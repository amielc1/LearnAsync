namespace LearnAsync.Tests
{
    [TestFixture]
    public class ChunkProcessorTests
    {
        private string outputFilePath;

        [SetUp]
        public void SetUp()
        {
            outputFilePath = Path.Combine(Path.GetTempPath(), "output.txt");
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
        }

        [Test]
        public async Task ProcessChunkAsync_WritesUniqueLinesToFile()
        {
            var chunkProcessor = new ChunkProcessor();
            var chunk = new List<string> { "line1", "line2", "line1" }; // "line1" is duplicated

            await chunkProcessor.ProcessChunkAsync(chunk, outputFilePath);

            var lines = await File.ReadAllLinesAsync(outputFilePath);

            Assert.That(lines.Length, Is.EqualTo(2)); // Only 2 unique lines should be written
            Assert.Contains("line1", lines);
            Assert.Contains("line2", lines);
        }
    }
}
