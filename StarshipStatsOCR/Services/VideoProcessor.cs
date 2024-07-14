using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCvSharp;
using StarshipStatsOCR.Models;

namespace StarshipStatsOCR.Services
{
    public class VideoProcessor
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly Func<IOcrEngine> _ocrEngineFactory;
        private readonly IDataValidator _dataValidator;
        private readonly IDataWriter _dataWriter;
        private readonly AppSettings _appSettings;

        public VideoProcessor(
            IImageProcessor imageProcessor,
            Func<IOcrEngine> ocrEngineFactory,
            IDataValidator dataValidator,
            IDataWriter dataWriter,
            AppSettings appSettings)
        {
            _imageProcessor = imageProcessor;
            _ocrEngineFactory = ocrEngineFactory;
            _dataValidator = dataValidator;
            _dataWriter = dataWriter;
            _appSettings = appSettings;
        }

        public void ProcessVideo()
        {
            using var capture = new VideoCapture(_appSettings.VideoPath);
            if (!capture.IsOpened())
            {
                Console.WriteLine("Could not open the video.");
                return;
            }

            var frameData = new List<string>();
            var tasks = new List<Task<string>>();
            var frame = new Mat();
            int frameCount = 0;

            while (capture.Read(frame))
            {
                frameCount++;
                var frameCopy = frame.Clone(); // Clonar el marco para evitar problemas de concurrencia
                int currentFrameCount = frameCount; // Capturar el número de marco actual para el contexto del hilo

                var task = Task.Run(() =>
                {
                    using var ocrEngine = _ocrEngineFactory(); // Crear una nueva instancia del motor OCR para cada tarea
                    var results = ProcessFrame(frameCopy, currentFrameCount, ocrEngine);
                    var line = string.Join(",", results);

                    Console.WriteLine(line);

                    return line;
                });

                tasks.Add(task);

                if (frameCount % 100 == 0)
                {
                    Console.WriteLine($"Processed {frameCount} frames");
                }
            }

            Task.WhenAll(tasks).ContinueWith(completedTasks =>
            {
                foreach (var task in completedTasks.Result)
                {
                    frameData.Add(task);
                }

                _dataWriter.WriteData(frameData, _appSettings.OutputPath);
                Console.WriteLine("Video processing completed.");
            }).Wait();
        }

        private string[] ProcessFrame(Mat frame, int frameCount, IOcrEngine ocrEngine)
        {
            var results = new string[4];
            results[0] = frameCount.ToString();

            for (int i = 0; i < _appSettings.Rois.Length; i++)
            {
                var roi = _appSettings.Rois[i];
                Mat croppedFrame = new Mat(frame, roi.ToRect());
                Mat processedFrame = _imageProcessor.PreprocessImage(croppedFrame);
                string text = ocrEngine.PerformOcr(processedFrame);
                results[i + 1] = _dataValidator.ValidateAndUpdateField(roi.Name, text);
            }

            return results;
        }
    }
}
