namespace ASP_MVC_NoAuthentication.Services
{
    public static class GoogleMapsService
    {
        public static string GetRouteUrl(string[] stops) // Build route url for stops - each stops has to be passed as coordinates, for example: "53.0703728,18.2621974"
        {
            string url = "https://www.google.com/maps/dir/";

            foreach(string coordinates in stops)
            {
                url += coordinates + "/";
            }

            return url;
        }
    }
}
