

using Plugins.Interfaces;

namespace Plugins
{
    public class FilesFoldersServices : IFilesFoldersServices
    {
        public void CopyDirectory(string source, string destination)
        {
            

            try
            {
                // Check if the source directory exists
                if (Directory.Exists(source))
                {
                    // Create the destination directory if it doesn't exist
                    if (!Directory.Exists(destination))
                    {
                        Directory.CreateDirectory(destination);
                    }

                    // Get the files in the source directory
                    string[] files = Directory.GetFiles(source);

                    foreach (string file in files)
                    {
                        // Get the destination file path
                        string destinationFile = Path.Combine(destination, Path.GetFileName(file));

                        // Copy the file to the destination directory
                        File.Copy(file, destinationFile, true);
                    }

                    // Get the subdirectories in the source directory
                    string[] subdirectories = Directory.GetDirectories(source);

                    foreach (string subdirectory in subdirectories)
                    {
                        // Get the destination subdirectory path
                        string destinationSubdirectory = Path.Combine(destination, Path.GetFileName(subdirectory));

                        // Recursively copy the subdirectory
                        CopyDirectory(subdirectory, destinationSubdirectory);
                    }

                    Console.WriteLine("Directory copied successfully.");
                }
                else
                {
                    Console.WriteLine("Source directory does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying directory: {ex.Message}");
            }
        }

    

    public void DeletePathIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    // Delete the directory and its contents
                    Directory.Delete(path, true);
                    Console.WriteLine("Directory deleted successfully.");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error deleting directory: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
            }
        }
    }
}
