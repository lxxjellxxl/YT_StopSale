

namespace Plugins.Interfaces
{
    public interface IJsonServices
    {
        public void UpdateValue(string[] nestedKeys, object newValue, string jsonFilePath);
    }
}
