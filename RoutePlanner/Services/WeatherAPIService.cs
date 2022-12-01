using RoutePlanner.Data;
using System.Text.Json;

namespace RoutePlanner.Services
{
    public class WeatherAPIService : IWeatherAPIService
    {
        private static readonly int retryCount = 5;
        private static readonly int retryWaitTime = 1500;
        private readonly ILogger<WeatherAPIService> logger;
        private readonly HttpClient client = new();
        private float latitude;
        private float longitude;
        private int retryNo = 1;
        

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
                    var currentDateTimeString = DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Day.ToString("00") + "T" + DateTime.Now.Hour.ToString("00") + ":00";

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
                logger.LogError($"Wystąpił wyjątek podczas pobierania temperatury (WeatherAPIService). Wyjątek: {ex.Message}\n{ex.InnerException}");
                if (retryNo <= retryCount)
                {
                    retryNo++;
                    logger.LogInformation($"Ponawianie próby.. próba nr. {retryNo}");
                    return await GetTemperatureForLocationAsync(lat, lng, estimatedTravelTime);
                }
                else
                {
                    return -300;
                }
            }
        }
    }
}
