namespace PSV.Utils;

public class TempFolderCleanerService : BackgroundService
{
    private readonly string _tempFolderPath;
    private readonly TimeSpan _interval = TimeSpan.FromHours(8);

    public TempFolderCleanerService(IWebHostEnvironment env)
    {
        _tempFolderPath = Path.Combine(env.WebRootPath, "temp");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            CleanTempFolder();
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private void CleanTempFolder()
    {
        if (Directory.Exists(_tempFolderPath))
        {
            var files = Directory.GetFiles(_tempFolderPath);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}