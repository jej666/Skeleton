namespace Skeleton.Abstraction
{
    public interface IConfigurationProvider : IHideObjectMethods
    {
        T GetConfigurationValue<T>(string key);
        string GetConfigurationValue(string key);
        T GetConfigurationValue<T>(string key, T defaultValue);
        string GetConfigurationValue(string key, string defaultValue);
    }
}