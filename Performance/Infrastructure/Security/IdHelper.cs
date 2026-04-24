using System.Buffers.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Performance.Application.Common.Settings;
using Performance.Application.Interface.Hashing;

namespace Performance.Infrastructure.Security;

public sealed class IdHelper(IOptions<IdEncryptionSettings> idHashingSettings) : IIdHelper
{
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const int IdSize = sizeof(long);

    private readonly byte[] _key = GetKey(idHashingSettings.Value);

    public string EncryptId(long id)
    {
        Span<byte> nonce = stackalloc byte[NonceSize];
        Span<byte> plain = stackalloc byte[IdSize];
        Span<byte> cipher = stackalloc byte[IdSize];
        Span<byte> tag = stackalloc byte[TagSize];
        Span<byte> output = stackalloc byte[NonceSize + IdSize + TagSize];

        RandomNumberGenerator.Fill(nonce);
        BitConverter.TryWriteBytes(plain, id);

        using var aes = new AesGcm(_key, TagSize);
        aes.Encrypt(nonce, plain, cipher, tag);

        nonce.CopyTo(output);
        cipher.CopyTo(output[NonceSize..]);
        tag.CopyTo(output[(NonceSize + IdSize)..]);

        return Base64Url.EncodeToString(output);
    }

    public long DecryptId(string encodedId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encodedId);

        Span<byte> raw = stackalloc byte[NonceSize + IdSize + TagSize];
        Span<byte> plain = stackalloc byte[IdSize];

        if (!Base64Url.TryDecodeFromChars(encodedId, raw, out int written) || written != raw.Length)
            throw new ArgumentException("Invalid encoded ID.");

        try
        {
            using var aes = new AesGcm(_key, TagSize);
            aes.Decrypt(
                nonce: raw[..NonceSize],
                ciphertext: raw[NonceSize..(NonceSize + IdSize)],
                tag: raw[(NonceSize + IdSize)..],
                plaintext: plain
            );
        }
        catch (CryptographicException ex)
        {
            throw new ArgumentException("Invalid, tampered, or mismatched ID.", ex);
        }

        return BitConverter.ToInt64(plain);
    }

    private static byte[] GetKey(IdEncryptionSettings settings)
    {
        if (string.IsNullOrEmpty(settings.Key))
            throw new InvalidOperationException("IdHashing key is not configured.");

        var key = Convert.FromBase64String(settings.Key);

        // AES-GCM requires 128, 192, or 256-bit keys
        if (key.Length is not (16 or 24 or 32))
            throw new InvalidOperationException($"IdHashing key must be 16, 24, or 32 bytes. Got {key.Length}.");

        return key;
    }
}