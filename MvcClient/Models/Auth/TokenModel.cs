namespace MvcClient.Models.Auth
{
    public class TokenModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string Expiration { get; set; }
    }
}