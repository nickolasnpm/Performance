// Infrastructure/Security/IdHelper.cs
using System.Buffers.Text;
using Microsoft.Extensions.Options;
using Performance.Application.Common.Settings;
using Performance.Application.Interface.Hashing;

namespace Performance.Infrastructure.Security
{
    public class IdHelper(IOptions<IdHashingSettings> idHashingSettings) : IIdHelper
    {
        private readonly byte[] _key = ValidateAndGetKey(idHashingSettings.Value);

        public string EncodeId(long id)
        {
            var shuffled = ShuffleId(id);
            var idBytes = BitConverter.GetBytes(shuffled);
            var obfuscatedBytes = ObfuscateId(idBytes);
            return Base64Url.EncodeToString(obfuscatedBytes);
        }

        public long DecodeId(string encodedId)
        {
            if (string.IsNullOrEmpty(encodedId))
                throw new ArgumentException("Encoded ID cannot be null or empty.");

            var obfuscatedBytes = Base64Url.DecodeFromChars(encodedId);
            var idBytes = ObfuscateId(obfuscatedBytes);
            var shuffled = BitConverter.ToInt64(idBytes);
            return UnshuffleId(shuffled);
        }

        private byte[] ObfuscateId(byte[] bytes)
        {
            var result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                result[i] = (byte)(bytes[i] ^ _key[i % _key.Length]);
            return result;
        }

        private static long ShuffleId(long id)
        {
            var unsigned = (ulong)id;
            unsigned = ((unsigned >> 16) ^ unsigned) * 0x45d9f3b37197344dUL;
            unsigned = ((unsigned >> 16) ^ unsigned) * 0x45d9f3b37197344dUL;
            unsigned = (unsigned >> 16) ^ unsigned;
            return (long)unsigned;
        }

        private static long UnshuffleId(long id)
        {
            var unsigned = (ulong)id;
            unsigned = ((unsigned >> 16) ^ unsigned) * 0x119de1f3a44d61b3UL;
            unsigned = ((unsigned >> 16) ^ unsigned) * 0x119de1f3a44d61b3UL;
            unsigned = (unsigned >> 16) ^ unsigned;
            return (long)unsigned;
        }

        private static byte[] ValidateAndGetKey(IdHashingSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Key))
                throw new InvalidOperationException("IdHashing key is not configured.");

            var key = Convert.FromBase64String(settings.Key);

            if (key.Length < 16)
                throw new InvalidOperationException("Key must be at least 16 bytes (128-bit).");

            return key;
        }
    }
}