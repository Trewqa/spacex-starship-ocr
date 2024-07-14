using System;
using System.Text.RegularExpressions;

namespace StarshipStatsOCR.Services
{
    public interface IDataValidator
    {
        string ValidateAndUpdateField(string fieldName, string value);
    }

    public class DataValidator : IDataValidator
    {
        private string _lastValidTime = "";
        private DateTime _lastValidDateTime = DateTime.MinValue;
        private string _lastValidSpeed = "";
        private int _lastValidSpeedValue = 0;
        private string _lastValidAltitude = "";
        private int _lastValidAltitudeValue = 0;

        public string ValidateAndUpdateField(string fieldName, string value)
        {
            return fieldName switch
            {
                "Time" => ValidateTime(value),
                "Speed" => ValidateSpeed(value),
                "Altitude" => ValidateAltitude(value),
                _ => value,
            };
        }

        private string ValidateTime(string value)
        {
            // Regex para validar el formato T[+-]hh:mm:ss
            if (Regex.IsMatch(value, @"^T([+-])(\d{2}):(\d{2}):(\d{2})$"))
            {
                var match = Regex.Match(value, @"^T([+-])(\d{2}):(\d{2}):(\d{2})$");
                string sign = match.Groups[1].Value;
                int hours = int.Parse(match.Groups[2].Value);
                int minutes = int.Parse(match.Groups[3].Value);
                int seconds = int.Parse(match.Groups[4].Value);

                // Validar que los valores estén dentro de los rangos permitidos
                if (hours <= 1 && minutes < 60 && seconds < 60)
                {
                    // Crear un DateTime para comparar con el último tiempo válido
                    DateTime currentTime = new DateTime(1, 1, 1, hours, minutes, seconds);
                    if (sign == "-")
                    {
                        currentTime = DateTime.MinValue.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
                    }

                    // Si es el primer tiempo válido o la diferencia es menor o igual a 5 segundos
                    if (_lastValidDateTime == DateTime.MinValue ||
                        Math.Abs((currentTime - _lastValidDateTime).TotalSeconds) <= 5)
                    {
                        _lastValidTime = value;
                        _lastValidDateTime = currentTime;
                        return value;
                    }
                }
            }
            return _lastValidTime;
        }

        private string ValidateSpeed(string value)
        {
            if (Regex.IsMatch(value, @"^(\d+) KM/H$"))
            {
                var match = Regex.Match(value, @"^(\d+) KM/H$");
                int currentSpeed = int.Parse(match.Groups[1].Value);

                if (_lastValidSpeed == "" || Math.Abs(currentSpeed - _lastValidSpeedValue) <= 200)
                {
                    _lastValidSpeed = value;
                    _lastValidSpeedValue = currentSpeed;
                    return value;
                }
            }
            return _lastValidSpeed;
        }

        private string ValidateAltitude(string value)
        {
            if (Regex.IsMatch(value, @"^(\d+) KM$"))
            {
                var match = Regex.Match(value, @"^(\d+) KM$");
                int currentAltitude = int.Parse(match.Groups[1].Value);

                if (_lastValidAltitude == "" || Math.Abs(currentAltitude - _lastValidAltitudeValue) <= 50)
                {
                    _lastValidAltitude = value;
                    _lastValidAltitudeValue = currentAltitude;
                    return value;
                }
            }
            return _lastValidAltitude;
        }
    }
}