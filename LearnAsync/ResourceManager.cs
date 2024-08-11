namespace LearnAsync
{
    public class ResourceManager
    {
        public void DeleteTempFile(string filePath)
        {
            lock (this)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
