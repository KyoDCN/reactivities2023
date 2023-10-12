namespace Reactivities.Infrastructure.Photos
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; } = string.Empty;
        public ApiSetting API { get; set; } = new ApiSetting();
    }

    public class ApiSetting
    {
        public string Key { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
