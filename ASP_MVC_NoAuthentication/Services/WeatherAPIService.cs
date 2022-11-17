using ASP_MVC_NoAuthentication.Data;
using System.Net;
using System.Security.Policy;
using System.Text.Json;

namespace ASP_MVC_NoAuthentication.Services
{
    public class WeatherAPIService : IWeatherAPIService
    {
        private readonly ILogger<WeatherAPIService> logger;
        private readonly HttpClient client = new();
        private float latitude;
        private float longitude;

        public WeatherAPIService(ILogger<WeatherAPIService> logger)
        {
            this.logger = logger;
        }

        private string Url
        {
            get
            {
                return "https://api.open-meteo.com/v1/forecast?latitude=" + latitude.ToString("0.00").Replace(",", ".") + "&longitude=" + longitude.ToString("0.00").Replace(",", ".") + "&hourly=temperature_2m";
            }
        }

        // Estimated travel time is passed in hours
        public async Task<float> GetTemperatureForLocationAsync(float lat, float lng, int estimatedTravelTime)
        {
            latitude = lat;
            longitude = lng;

            try
            {
                // Get API response
                HttpResponseMessage response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON to object
                JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
                WeatherAPIResponse? weatherAPIResponseObject = JsonSerializer.Deserialize<WeatherAPIResponse>(responseBody, jsonSerializerOptions);

                if (weatherAPIResponseObject != null)
                {
                    var currentDateTimeString = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "T" + DateTime.Now.Hour.ToString("00") + ":00";

                    int i;
                    for (i = 0; i < weatherAPIResponseObject.Hourly.Time.Count; i++)
                    {
                        if (string.Equals(currentDateTimeString, weatherAPIResponseObject.Hourly.Time[i]))
                            break;
                        else if (i == weatherAPIResponseObject.Hourly.Time.Count - 1)
                            throw new Exception("Problem z dopasowaniem godziny.");
                    }

                    double temperature = 0;
                    int j;
                    for (j = 0; j <= estimatedTravelTime; j++)
                    {
                        temperature += weatherAPIResponseObject.Hourly.Temperature_2M[i];
                        i++;
                    }

                    temperature = temperature / (j + 1);
                    return (float)temperature;
                }
                else
                    throw new Exception("Odpowiedź od Weather API jest nullem.");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Wystąpił wyjątek podczas pobierania temperatury (WeatherAPIService). Wyjątek: {ex.Message}\n{ex.InnerException}");
                return -300;
            }
        }
    }
}
