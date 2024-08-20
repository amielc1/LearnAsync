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

        //[Test]
        //public async Task GroupLinesByFirstCharacterAsync_CreatesTempFilesByCharacter()
        //{
        //    var tempDirectory = Path.Combine(Path.GetTempPath(), "tempDir");
        //    var fileChunkReader = new FileReader();

        //    await fileChunkReader.GroupLinesByHashAsync(inputFilePath, tempDirectory,10);

        //    Assert.IsTrue(Directory.Exists(tempDirectory));

        //    var files = Directory.GetFiles(tempDirectory);
        //    Assert.That(files.Length, Is.EqualTo(2));

        //    Assert.IsTrue(File.Exists(Path.Combine(tempDirectory, "A.txt")));
        //    Assert.IsTrue(File.Exists(Path.Combine(tempDirectory, "B.txt")));
        //}

    }
}
