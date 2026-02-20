namespace TodoCLI.Logger;

public static class Logger
{
    private static readonly string LogsFolder = Path.Combine(AppContext.BaseDirectory, "Logs");
    private static readonly string LogFilePath = Path.Combine(LogsFolder, "log.txt");

    public static void Log(Exception ex, string? contextMessage = null)
    {
        try
        {
            if (!Directory.Exists(LogsFolder))
            {
                Directory.CreateDirectory(LogsFolder);
            }

            string logEntry = $"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss}]";

            if (!string.IsNullOrWhiteSpace(contextMessage))
            {
                logEntry += $" Context: {contextMessage}";
            }

            logEntry += Environment.NewLine;
            logEntry += $"Exception: {ex.GetType().FullName}" + Environment.NewLine;
            logEntry += $"Message  : {ex.Message}" + Environment.NewLine;
            logEntry += $"StackTrace:{Environment.NewLine}{ex.StackTrace}" + Environment.NewLine;
            logEntry += new string('-', 60) + Environment.NewLine;

            File.AppendAllText(LogFilePath, logEntry);
        }
        catch
        {
           
        }
    }
}