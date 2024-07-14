using Microsoft.Extensions.Configuration;
using StarshipStatsOCR.Services;
using StarshipStatsOCR.Models;
namespace StarshipStatsOCR
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var appSettings = configuration.Get<AppSettings>();

            var imageProcessor = new ImageProcessor();
            var dataValidator = new DataValidator();
            var dataWriter = new CsvDataWriter();

            Func<IOcrEngine> ocrEngineFactory = () => new TesseractOcrEngine(appSettings.TessdataPath);

            var videoProcessor = new VideoProcessor(
                imageProcessor,
                ocrEngineFactory,
                dataValidator,
                dataWriter,
                appSettings
            );

            videoProcessor.ProcessVideo();
        }
    }
}
