namespace LearnAsync.Tests
{
    [TestFixture]
    public class FileChunkReaderTests
    {
        private string inputFilePath;

        [SetUp]
        public void SetUp()
        {
            inputFilePath = Path.Combine(Path.GetTempPath(), "input.txt");
            File.WriteAllLines(inputFilePath, new[] { "apple", "banana", "apricot", "berry" });
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(inputFilePath))
            {
                File.Delete(inputFilePath);
            }
        }

        [Test]
        public async Task GroupLinesByFirstCharacterAsync_CreatesTempFilesByCharacter()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), "tempDir");
            var fileChunkReader = new FileChunkReader();

            await fileChunkReader.GroupLinesByFirstCharacterAsync(inputFilePath, tempDirectory);

            Assert.IsTrue(Directory.Exists(tempDirectory));

            var files = Directory.GetFiles(tempDirectory);
            Assert.That(files.Length, Is.EqualTo(2)); // Should have files for 'A' and 'B'

            Assert.IsTrue(File.Exists(Path.Combine(tempDirectory, "A.txt")));
            Assert.IsTrue(File.Exists(Path.Combine(tempDirectory, "B.txt")));

            // Clean up
            Directory.Delete(tempDirectory, true);
        }

        [Test]
        public async Task ReadChunksAsync_ReturnsChunksOfLines()
        {
            var fileChunkReader = new FileChunkReader();
            var tempFilePath = Path.Combine(Path.GetTempPath(), "testFile.txt");
            await File.WriteAllLinesAsync(tempFilePath, new[] { "line1", "line2", "line3" });

            var chunks = new List<List<string>>();
            await foreach (var chunk in fileChunkReader.ReadChunksAsync(tempFilePath, 10))
            {
                chunks.Add(chunk);
            }

            Assert.That(chunks.Count, Is.EqualTo(3)); // Each line is less than 10 bytes, so each line is a separate chunk

            // Clean up
            File.Delete(tempFilePath);
        }
    }
}
