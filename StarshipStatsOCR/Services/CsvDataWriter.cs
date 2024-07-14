namespace StarshipStatsOCR.Services
{
    public interface IDataWriter
    {
        void WriteData(IEnumerable<string> data, string outputPath);
    }

    public class CsvDataWriter : IDataWriter
    {
        public void WriteData(IEnumerable<string> data, string outputPath)
        {
            File.WriteAllLines(outputPath, data);
        }
    }
}