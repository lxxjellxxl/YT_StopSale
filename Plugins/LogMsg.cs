using Entities;
using Microsoft.Extensions.Options;
using Plugins.Interfaces;

namespace Entities
{
    public class LogMsg
    {
        private static string logName = "log - " + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt";
        private readonly InputConfig config;
        public LogMsg(IOptions<InputConfig> config)
        {
            this.config = config.Value;
            RefreshLogFolder();
            EnsureLogFileExists();
        }

        public string LogFilePath()
        {
            string logPath = config.LogPath;
            string logFilePath = Path.Combine(logPath, logName);
            return logFilePath;
        }

        public void RefreshLogFolder()
        {
            string logsFolderPath = config.LogPath;

            if (Directory.Exists(logsFolderPath))
            {
                Directory.Delete(logsFolderPath, true);
            }
            Directory.CreateDirectory(logsFolderPath);

        }
        public void EnsureLogFileExists()
        {
            if (!File.Exists(LogFilePath()))
            {
                using (StreamWriter sw = File.CreateText(LogFilePath()))
                {
                    sw.WriteLine("Log File Created at " + DateTime.Now);
                }
            }
        }

        public void Log(string message)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(LogFilePath()))
                {
                    writer.WriteLine($"{DateTime.Now} - {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while logging: {ex.Message}");
            }
        }


    }
}