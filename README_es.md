# StarshipStatsOCR

[README in English](https://github.com/Trewqa/spacex-starship-ocr/edit/master/README.md)

StarshipStatsOCR es una herramienta avanzada de Reconocimiento Óptico de Caracteres (OCR) diseñada para extraer y procesar datos de telemetría de videos de lanzamiento de SpaceX Starship. Este proyecto tiene como objetivo proporcionar una extracción de datos precisa y en tiempo real para la velocidad, altitud y tiempo transcurrido de la misión a partir de transmisiones de video.

## Planteamiento del problema

Durante los lanzamientos de SpaceX Starship, los datos de telemetría se muestran en pantalla, pero no siempre están disponibles en un formato de archivo para su posterior analisis. Este proyecto resuelve el desafío de extraer estos valiosos datos para análisis, investigación y propósitos educativos.

## Cómo funciona

StarshipStatsOCR utiliza tecnologías de visión por ordenador y OCR para procesar fotogramas de video, extraer datos relevantes y producirlos en un formato estructurado. El sistema emplea varios mecanismos de validación para garantizar la precisión y consistencia de los datos.

## Características

- **Procesamiento de video**: Procesa eficientemente archivos de video fotograma por fotograma.
- **Extracción de Región de Interés (ROI)**: Se enfoca en áreas específicas del fotograma de video donde se muestran los datos de telemetría.
- **Preprocesamiento de imagen**: Mejora la calidad de la imagen para una mayor precisión del OCR.
- **Integración de OCR**: Utiliza el motor Tesseract OCR para el reconocimiento de texto.
- **Validación de datos**: Implementa robustas verificaciones de validación para datos de tiempo, velocidad y altitud.
- **Verificación de consistencia temporal**: Asegura que los valores de tiempo extraídos estén dentro de un rango de 5 segundos del tiempo válido anterior.
- **Verificación de variación de velocidad y altitud**: Valida que los cambios de velocidad y altitud no excedan las 50 unidades entre lecturas consecutivas.
- **Salida CSV**: Genera un archivo CSV estructurado con datos extraídos y validados.
- **Diseño modular**: Utiliza una arquitectura modular para facilitar el mantenimiento y la extensibilidad.
- **Gestión de configuración**: Emplea una configuración basada en JSON para una fácil personalización de rutas de video, ROIs y otras configuraciones.

## Instalación

```bash
git clone https://github.com/Trewqa/spacex-starship-ocr.git
cd StarshipStatsOCR
dotnet restore
```

Asegúrate de tener instaladas las siguientes dependencias:
- .NET 6.0 o posterior
- OpenCVSharp4
- Motor Tesseract OCR

## Uso

1. Configura tus ajustes en `appsettings.json`.
2. Ejecuta la aplicación:

```bash
dotnet run
```

## Configuración

Edita `appsettings.json` para establecer la ruta de tu video, la ruta de datos de Tesseract, la ruta del archivo de salida y las Regiones de Interés (ROIs).

```json
{
  "VideoPath": "ruta/a/tu/video.mp4",
  "TessdataPath": "./tessdata",
  "OutputPath": "salida.csv",
  "Rois": [
    {
      "Name": "Velocidad",
      "X": 306,
      "Y": 906,
      "Width": 198,
      "Height": 40
    },
    // ... otras ROIs
  ]
}
```

## Áreas de mejora

Aunque StarshipStatsOCR es funcional, hay varias áreas donde se podría mejorar:

- **Integración de Aprendizaje Automático**: Implementar modelos de ML para mejorar el reconocimiento de texto y la validación de datos.
- **GUI**: Desarrollar una interfaz gráfica de usuario para una operación más sencilla.
- **Manejo de errores**: Mejorar los mecanismos de manejo de errores y registro.
- **Pruebas unitarias**: Aumentar la cobertura de pruebas para una mayor fiabilidad.

## Contribuciones

Por favor, lee [CONTRIBUTING.md](CONTRIBUTING.md) para conocer los detalles del código de conducta y el proceso para enviar pull requests.

## Licencia

Este proyecto está licenciado bajo la Licencia MIT - consulta el archivo [LICENSE.md](LICENSE.md) para más detalles.

## Agradecimientos

- A SpaceX por inspirar este proyecto a través de su innovador programa Starship.
- A la comunidad de código abierto por proporcionar herramientas y bibliotecas que han permitido crear este proyecto.

## Contacto

Enlace del Proyecto: [https://github.com/Trewqa/spacex-starship-ocr](https://github.com/Trewqa/spacex-starship-ocr)

---

Hecho con ❤️ para la exploración espacial y la ciencia abierta.
