
using MailKit.Search;
using Newtonsoft.Json;
using Recipit.Infrastructure.Extensions.Contracts;
using System.Net.Http;

namespace Recipit.Services.ImageWebSearch
{
    public class GoogleImageSearchService(IConfiguration configuration) : ISearchService
    {
        private readonly HttpClient _httpClient = new();
        private readonly IConfiguration _configuration = configuration;

        public async Task<string> GetImageUrlByName(string searchQuery)
        {
            var googleSearchApi = _configuration.GetSection(nameof(GoogleSearchApi)).Get<GoogleSearchApi>();

            ArgumentNullException.ThrowIfNull(googleSearchApi);

            var requestUri = string.Format(googleSearchApi.Url, Uri.EscapeDataString(searchQuery), googleSearchApi.SearchEngineId, googleSearchApi.Key);

            var response = await _httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var searchResults = JsonConvert.DeserializeObject<GoogleSearchResponse>(responseContent);

                ArgumentNullException.ThrowIfNull(searchResults);

                if (searchResults.Items is not null && searchResults.Items.Count != 0)
                {
                    foreach(var item in searchResults.Items.Select(a => a.Link))
                    {
                        if (item.Split('.').Last().ToCharArray().Length <= 4)
                        {
                            return item;
                        }
                    }
                }
            }

            throw new ArgumentNullException(nameof(searchQuery));
        }
    }

    public class GoogleSearchResponse
    {
        [JsonProperty("items")]
        public List<SearchResultItem> Items { get; set; } = default!;
    }

    public class SearchResultItem
    {
        [JsonProperty("link")]
        public string Link { get; set; } = default!;
    }
}
