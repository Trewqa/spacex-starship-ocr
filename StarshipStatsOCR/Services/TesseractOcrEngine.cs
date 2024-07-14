using OpenCvSharp;
using Tesseract;

namespace StarshipStatsOCR.Services
{
    public interface IOcrEngine : IDisposable
    {
        string PerformOcr(Mat img);
    }

    public class TesseractOcrEngine : IOcrEngine
    {
        private readonly TesseractEngine _engine;

        public TesseractOcrEngine(string tessdataPath)
        {
            _engine = new TesseractEngine(tessdataPath, "eng", EngineMode.LstmOnly);
            _engine.SetVariable("user_defined_dpi", 300);
            //_engine.DefaultPageSegMode = PageSegMode.SingleBlock;
            //_engine.DefaultPageSegMode = PageSegMode.SingleLine;
        }

        public string PerformOcr(Mat img)
        {
            using var pix = Pix.LoadFromMemory(img.ToBytes());
            using var page = _engine.Process(pix);
            return page.GetText().Trim();
        }

        public void Dispose()
        {
            _engine?.Dispose();
        }
    }
}