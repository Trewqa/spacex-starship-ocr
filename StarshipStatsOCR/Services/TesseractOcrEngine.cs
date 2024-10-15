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
            _engine.SetVariable("tessedit_char_whitelist", "0123456789KM/H +-T:");

            _engine.SetVariable("tessedit_write_images", "0");  // No guardar imágenes intermedias
            _engine.SetVariable("textord_debug_tabfind", "0");  // Deshabilitar salida de logs relacionados con tablas
            _engine.SetVariable("debug_file", "/dev/null");     // Redirigir el log a un lugar vacío (o puedes usar NUL en Windows)

            //_engine.DefaultPageSegMode = PageSegMode.SingleBlock;
            _engine.DefaultPageSegMode = PageSegMode.SingleLine;
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