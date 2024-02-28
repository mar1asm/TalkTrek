namespace Learning_platform.Services
{
    public class EmailConfirmationClient
    {
        private readonly HttpClient _httpClient;

        public EmailConfirmationClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string confirmationCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7048/confirm-email")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("userId", userId),
                new KeyValuePair<string, string>("confirmationCode", confirmationCode)
            })
            };

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
