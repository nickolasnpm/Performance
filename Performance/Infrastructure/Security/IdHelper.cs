using System.Buffers.Text;
using System.Text;
using Microsoft.Extensions.Options;
using Performance.Application.Common.Settings;
using Performance.Application.Interface.Hashing;

namespace Performance.Infrastructure.Security
{
    public class IdHelper(IOptions<IdHashingSettings> idHashingSettings) : IIdHelper
    {
        private const ulong SplitMix64ShuffleMul1   = 0xbf58476d1ce4e5b9UL;
        private const ulong SplitMix64ShuffleMul2   = 0x94d049bb133111ebUL;
        private const ulong SplitMix64UnshuffleMul1 = 0x319642b2d24d8ec3UL;
        private const ulong SplitMix64UnshuffleMul2 = 0x96de1b173f119089UL;

        private readonly byte[] _key = ValidateAndGetKey(idHashingSettings.Value);

        public string EncodeId(long id, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("Prefix cannot be null or empty.");

            var prefixBytes = Encoding.UTF8.GetBytes(prefix);
            var idBytes = BitConverter.GetBytes(ShuffleId(id));

            for (int i = 0; i < prefixBytes.Length; i++)
                idBytes[i % idBytes.Length] ^= prefixBytes[i];

            return Base64Url.EncodeToString(ObfuscateBytes(idBytes));
        }

        public long DecodeId(string encodedId, string expectedPrefix)
        {
            if (string.IsNullOrEmpty(encodedId))
                throw new ArgumentException("Encoded ID cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(expectedPrefix))
                throw new ArgumentException("Expected prefix cannot be null or empty.");

            var idBytes = ObfuscateBytes(Base64Url.DecodeFromChars(encodedId));
            var prefixBytes = Encoding.UTF8.GetBytes(expectedPrefix);

            for (int i = 0; i < prefixBytes.Length; i++)
                idBytes[i % idBytes.Length] ^= prefixBytes[i];

            return UnshuffleId(BitConverter.ToInt64(idBytes));
        }

        private byte[] ObfuscateBytes(byte[] bytes)
        {
            var result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                result[i] = (byte)(bytes[i] ^ _key[i % _key.Length]);
            return result;
        }

        private static long ShuffleId(long id)
        {
            var u = (ulong)id;
            u ^= u >> 30; u *= SplitMix64ShuffleMul1;
            u ^= u >> 27; u *= SplitMix64ShuffleMul2;
            u ^= u >> 31;
            return (long)u;
        }

        private static long UnshuffleId(long id)
        {
            var u = (ulong)id;
            u ^= (u >> 31) ^ (u >> 62); u *= SplitMix64UnshuffleMul1;
            u ^= (u >> 27) ^ (u >> 54); u *= SplitMix64UnshuffleMul2;
            u ^= (u >> 30) ^ (u >> 60);
            return (long)u;
        }

        private static byte[] ValidateAndGetKey(IdHashingSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Key))
                throw new InvalidOperationException("IdHashing key is not configured.");

            return Convert.FromBase64String(settings.Key);
        }
    }
}