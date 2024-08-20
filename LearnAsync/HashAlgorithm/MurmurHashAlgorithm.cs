using Murmur;
using System.Text;

namespace LearnAsync.HashAlgorithm
{

    public class MurmurHashAlgorithm : IHashAlgorithm
    {
        private readonly Murmur32 _murmur32;

        public MurmurHashAlgorithm()
        {
            _murmur32 = MurmurHash.Create32();
        }

        public int ComputeHash(string input)
        {
            var hashBytes = _murmur32.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToInt32(hashBytes, 0);
        }
    }

}
