using OpenCvSharp;

namespace StarshipStatsOCR.Models
{
    public class AppSettings
    {
        public string VideoPath { get; set; }
        public string TessdataPath { get; set; }
        public string OutputPath { get; set; }
        public RoiSettings[] Rois { get; set; }
    }

    public class RoiSettings
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rect ToRect() => new Rect(X, Y, Width, Height);
    }
}