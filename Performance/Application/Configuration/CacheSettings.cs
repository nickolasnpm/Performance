namespace Performance.Application.Configuration
{
    public class CacheSettings
    {
        public Dictionary<string, CacheItemSettings> Items { get; set; } = new();
    }

    public class CacheItemSettings
    {
        public string Key { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; }
    }

    public static class CacheKeys
    {
        public const string UserCount = "UserCount";
    }

}
