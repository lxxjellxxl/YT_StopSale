

namespace Plugins.Interfaces
{
    public interface IFilesFoldersServices
    {
        public void DeletePathIfExists(string path);
        public void CopyDirectory(string source, string destination);
    }
}
