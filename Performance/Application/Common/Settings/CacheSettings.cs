using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace Performance.Application.Common.Settings
{
    public sealed class CacheSettings
    {
        public bool Enabled { get; init; }

        [ValidateObjectMembers]
        public CacheItemSettings UserCount { get; init; } = new();
    }

    public sealed class CacheItemSettings
    {
        [Range(1, int.MaxValue, ErrorMessage = "ExpirationMinutes must be > 0")]
        public int ExpirationMinutes { get; init; }
    }
}