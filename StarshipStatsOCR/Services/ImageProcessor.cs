using OpenCvSharp;

namespace StarshipStatsOCR.Services
{
    public interface IImageProcessor
    {
        Mat PreprocessImage(Mat img);
    }

    public class ImageProcessor : IImageProcessor
    {
        public Mat PreprocessImage(Mat img)
        {
            Mat grayFrame = new Mat();
            Cv2.CvtColor(img, grayFrame, ColorConversionCodes.BGR2GRAY);

            Mat contrastFrame = new Mat();
            Cv2.ConvertScaleAbs(grayFrame, contrastFrame, 1.5, 0);

            Mat mask = new Mat();
            Cv2.InRange(contrastFrame, new Scalar(245, 245, 245), new Scalar(255, 255, 255), mask);

            Mat filteredFrame = new Mat();
            Cv2.BitwiseAnd(contrastFrame, contrastFrame, filteredFrame, mask);

            Mat thresholdFrame = new Mat();
            Cv2.Threshold(filteredFrame, thresholdFrame, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

            return thresholdFrame;
        }
    }
}