namespace StoreApi.Helpers
{
    public static class RequestExtentions
    { 
        public static string BaseUrl(this HttpRequest httpRequest)
        {
            return $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
        }
    }
}
