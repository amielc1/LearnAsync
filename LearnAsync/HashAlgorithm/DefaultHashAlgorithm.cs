namespace LearnAsync.HashAlgorithm
{
    public class DefaultHashAlgorithm : IHashAlgorithm
    {
        public int ComputeHash(string input)
        {
            return input.GetHashCode();
        }
    }

}
