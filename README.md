# StarshipStatsOCR

[README en Español](https://github.com/Trewqa/spacex-starship-ocr/blob/master/README_es.md)

StarshipStatsOCR is an advanced Optical Character Recognition (OCR) tool designed to extract and process telemetry data from SpaceX Starship launch videos. This project aims to provide accurate and real-time data extraction for mission speed, altitude, and elapsed time from video broadcasts.

## Problem Statement

During SpaceX Starship launches, telemetry data is displayed on screen but is not always available in a file format for subsequent analysis. This project solves the challenge of extracting this valuable data for analysis, research, and educational purposes.

## How It Works

StarshipStatsOCR uses computer vision and OCR technologies to process video frames, extract relevant data, and output it in a structured format. The system employs various validation mechanisms to ensure data accuracy and consistency.

## Features

- **Video Processing**: Efficiently processes video files frame by frame.
- **Region of Interest (ROI) Extraction**: Focuses on specific areas of the video frame where telemetry data is displayed.
- **Image Preprocessing**: Enhances image quality for improved OCR accuracy.
- **OCR Integration**: Uses the Tesseract OCR engine for text recognition.
- **Data Validation**: Implements robust validation checks for time, speed, and altitude data.
- **Temporal Consistency Verification**: Ensures that extracted time values are within a 5-second range of the previous valid time.
- **Speed and Altitude Variation Check**: Validates that speed and altitude changes do not exceed 50 units between consecutive readings.
- **CSV Output**: Generates a structured CSV file with extracted and validated data.
- **Modular Design**: Uses a modular architecture for easy maintenance and extensibility.
- **Configuration Management**: Employs JSON-based configuration for easy customization of video paths, ROIs, and other settings.

## Installation

```bash
git clone https://github.com/Trewqa/spacex-starship-ocr.git
cd StarshipStatsOCR
dotnet restore
```

Make sure you have the following dependencies installed:
- .NET 6.0 or later
- OpenCVSharp4
- Tesseract OCR engine

## Usage

1. Configure your settings in appsettings.json.
2. Run the application:

```bash
dotnet run
```

## Configuration

Edit appsettings.json to set your video path, Tesseract data path, output file path, and Regions of Interest (ROIs).

```json
{
  "VideoPath": "path/to/your/video.mp4",
  "TessdataPath": "./tessdata",
  "OutputPath": "output.csv",
  "Rois": [
    {
      "Name": "Speed",
      "X": 306,
      "Y": 906,
      "Width": 198,
      "Height": 40
    },
    // ... other ROIs
  ]
}
```

## Areas for Improvement

While StarshipStatsOCR is functional, there are several areas where it could be improved:

- **Machine Learning Integration**: Implement ML models to enhance text recognition and data validation.
- **GUI**: Develop a graphical user interface for easier operation.
- **Error Handling**: Improve error handling and logging mechanisms.
- **Unit Testing**: Increase test coverage for greater reliability.

## Contributions

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

- SpaceX for inspiring this project through their innovative Starship program.
- The open-source community for providing tools and libraries that have made this project possible.

## Contact

Project Link: [https://github.com/Trewqa/spacex-starship-ocr](https://github.com/Trewqa/spacex-starship-ocr)

---

Made with ❤️ for space exploration and open science.
