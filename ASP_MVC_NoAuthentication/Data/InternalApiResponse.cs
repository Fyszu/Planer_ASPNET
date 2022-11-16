using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ASP_MVC_NoAuthentication.Data
{
    public class InternalApiResponse
    {
        public StatusCode Status { get; set; }
        public string ErrorMessage { get; set; }
        public string? Response {  get; set; }
        public enum StatusCode
        {
            None,
            OK,
            GoogleNoResults,
            GoogleInvalidApiKey,
            GoogleUnauthorizedApiKey,
            GoogleRequestDenied,
            GoogleLimitReached,
            GoogleServerSideUnknownError,
            GoogleNoParametersPassedError,
            GoogleInvalidRequest,
            InternalLimitReached,
            DistanceControllerWeatherAPIError,
            DistanceControllerOtherError,
            HostNotAllowed,
            UnknownError
        }

        private Dictionary<StatusCode, string> errorMessagesDictionary = new()
        {
            { StatusCode.GoogleInvalidApiKey, "Błąd po stronie aplikacji - nieprawidłowy klucz Google API." },
            { StatusCode.GoogleUnauthorizedApiKey, "Błąd po stronie aplikacji - nieautoryzowany klucz Google API." },
            { StatusCode.GoogleRequestDenied, "Błąd po stronie aplikacji - odmowa dostępu od strony google." },
            { StatusCode.GoogleLimitReached, "Zbyt wiele zapytań w krótkim czasie lub przekroczono ustalony dzienny limit zapytań dla Google API. Spróbuj ponownie, a jeśli błąd będzie nadal występował wróć jutro." },
            { StatusCode.GoogleServerSideUnknownError, "Wystąpił błąd po stronie serwera Google podczas przetwarzania adresu. Spróbuj ponownie później." },
            { StatusCode.GoogleNoParametersPassedError, "Błąd po stronie aplikacji - nie przekazano parametrów zapytania." },
            { StatusCode.GoogleInvalidRequest, "Błąd po stronie aplikacji - złe zapytanie." },
            { StatusCode.DistanceControllerWeatherAPIError, "Błąd po stronie aplikacji - błąd podczas przetwarzania temperatury z zewnętrznego API." },
            { StatusCode.DistanceControllerOtherError, "Błąd po stronie aplikacji - błąd podczas przetwarzania wyniku obliczeń dystansu samochodu." },
            { StatusCode.UnknownError, "Wystąpił nieznany błąd. Spróbuj ponownie." }
        }; 
        public InternalApiResponse(StatusCode status, string? response)
        {
            Status = status;
            Response = response;
        }

        public string GetInternalResponseJson()
        {
            string? errorMessage = errorMessagesDictionary.Where(pair => pair.Key == Status).FirstOrDefault().Value;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorMessage = errorMessage;
            }
            Response ??= string.Empty;
            ErrorMessage ??= string.Empty;
            var result = JsonConvert.SerializeObject(this);
            result = result.Replace("\\", "");
            if (!string.IsNullOrEmpty(Response))
                result = result.Replace("\"" + Response + "\"", Response);
            return result;
        }

        // Geocoding documentation: https://developers.google.com/maps/documentation/geocoding/requests-geocoding?hl=pl
    }
}
