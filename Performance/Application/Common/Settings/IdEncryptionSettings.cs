using System.ComponentModel.DataAnnotations;

namespace Performance.Application.Common.Settings
{
    public class IdEncryptionSettings
    {
        [Required, MinLength(1)]
        public string Key { get; init; } = string.Empty;
    }
}