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

                    // Comprobamos las condiciones necesarias
                    if (results.Length < 6)
                    {
                        // Si el arreglo no contiene la cantidad necesaria de elementos, ignoramos la línea
                        Console.WriteLine("1");
                        return null;
                    }

                    // Comprobación de que result[0] es un número
                    if (!double.TryParse(results[0], out _))
                    {
                        // Si result[0] no es un número, ignoramos la línea
                        Console.WriteLine("2");
                        return null;
                    }

                    // Comprobación de que result[1] termina en "KM/H" y antes de eso es un número
                    if (!results[1].EndsWith(" KM/H") || !double.TryParse(results[1].Replace(" KM/H", ""), out _))
                    {
                        // Si result[1] no contiene un valor de velocidad válido, ignoramos la línea
                        Console.WriteLine("3");
                        return null;
                    }

                    // Comprobación de que result[2] es un número con "KM"
                    if (!results[2].EndsWith(" KM") || !double.TryParse(results[2].Replace(" KM", ""), out _))
                    {
                        // Si result[2] no contiene una distancia válida, ignoramos la línea
                        Console.WriteLine("4 " + results[2]);
                        return null;
                    }

                    // Comprobación de que result[5] corresponde al formato de tiempo T+XX:XX:XX
                    var timePattern = @"^T\+\d{2}:\d{2}:\d{2}$";
                    if (!System.Text.RegularExpressions.Regex.IsMatch(results[5], timePattern))
                    {
                        // Si result[5] no corresponde al formato de tiempo, ignoramos la línea
                        Console.WriteLine("5 " + results[5]);
                        return null;
                    }

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
            var results = new string[6];
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
