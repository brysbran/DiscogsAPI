using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscogsAPIConsole
{
    /// <summary>
    /// Class that will perform the functionalities of the API
    /// Will contain methods that send requests to the API and retrieve that information as JSON, then convert it to readable text.
    /// </summary>
    public class DiscogsService
    {
        private const string BaseUrl = "https://api.discogs.com";
        private readonly string _apiToken = "aXsNFZYDhQneBFDJOQwHOzSGChVHkNnpCFxtORRg";
        private HttpClient _httpClient;


        public DiscogsService(string apiToken)
        {
            _apiToken = apiToken;
            _httpClient = new HttpClient();
            InitializeClient();
        }

        private void InitializeClient()
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DiscogsAPIConsole/1.0");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Discogs token={_apiToken}");
        }

        public async Task<string> GetAsync(string endpoint)
        {
            return await _httpClient.GetStringAsync($"{BaseUrl}/{endpoint}");
        }

        /// <summary>
        /// Gets the release information for a particular ID.
        /// </summary>
        /// <param name="releaseId"></param>
        /// <returns></returns>
        public async Task<JObject> GetRelease(int releaseId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DiscogsAPIConsole/1.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Discogs token={_apiToken}");

                var response = await client.GetStringAsync($"{BaseUrl}/releases/{releaseId}");
                return JObject.Parse(response);
            }
        }

        /// <summary>
        /// The method uses the search endpoint to find artists by name. If there are multiple artists with similar names,
        /// the method will retrieve the information of the first artist in the search results.
        /// Need to convert the raw JSON to a C# object.
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        public async Task<Artist> GetArtistByName(string artistName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DiscogsAPIConsole/1.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Discogs token={_apiToken}");

                var searchResponse = await client.GetStringAsync($"{BaseUrl}/database/search?q={Uri.EscapeDataString(artistName)}&type=artist");
                var searchResults = JObject.Parse(searchResponse);

                if (searchResults["results"] != null && searchResults["results"].HasValues)
                {
                    var firstArtistId = searchResults["results"][0]["id"].Value<int>();
                    var artistResponse = await client.GetStringAsync($"{BaseUrl}/artists/{firstArtistId}");

                    return JsonConvert.DeserializeObject<Artist>(artistResponse);
                }

                return null;
            }
        }

        /// <summary>
        /// Allows a user to search for a release based on a string query, and automatically returns the 1st page.
        /// Also deserializes the result as a ReleaseSearchResponse object.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<ReleaseSearchResponse> SearchRelease(string query, int pageNumber = 1)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DiscogsAPIConsole/1.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Discogs token={_apiToken}");

                var response = await client.GetStringAsync($"{BaseUrl}/database/search?q={Uri.EscapeDataString(query)}&type=release&page={pageNumber}");
                return JsonConvert.DeserializeObject<ReleaseSearchResponse>(response);
            }
        }

        /// <summary>
        /// Fetches the label information based on ID
        /// returns type of Label which contains the information.
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns></returns>
        public async Task<Label> GetLabel(int labelId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DiscogsAPIConsole/1.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Discogs token={_apiToken}");

                var response = await client.GetStringAsync($"{BaseUrl}/labels/{labelId}");
                return JsonConvert.DeserializeObject<Label>(response);
            }
        }

        /// <summary>
        /// Fetches the marketplace listings of a search by page
        /// Currently only displays one page, would need to loop through API calls to show multiple
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<MarketplaceSearchResponse> GetMarketplaceListings(string query, int pageNumber = 1)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DiscogsAPIConsole/1.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Discogs token={_apiToken}");

                var response = await client.GetStringAsync($"{BaseUrl}/marketplace/listings?q={Uri.EscapeDataString(query)}&page={pageNumber}");
                return JsonConvert.DeserializeObject<MarketplaceSearchResponse>(response);
            }
        }

        /// <summary>
        /// Retrieves all items in a users collection by username
        /// loops through all pages of items
        /// </summary>
        /// <param name="username"></param>
        /// <param name="folderId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<List<CollectionRelease>> GetUserCollectionItems(string username, int folderId = 0)
        {
            List<CollectionRelease> allCollectionItems = new List<CollectionRelease>();
            int pageNumber = 1;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "DiscogsAPIConsole/1.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Discogs token={_apiToken}");

                while (true)
                {
                    var response = await client.GetStringAsync($"{BaseUrl}/users/{username}/collection/folders/{folderId}/releases?page={pageNumber}");
                    var collectionResponse = JsonConvert.DeserializeObject<UserCollectionResponse>(response);

                    // Add the current page's collection items to the list
                    allCollectionItems.AddRange(collectionResponse.Releases);

                    // Check if there are more pages to retrieve
                    if (pageNumber >= collectionResponse.Pagination.Pages)
                    {
                        break; // No more pages, exit the loop
                    }

                    pageNumber++; // Move to the next page
                }
            }

            return allCollectionItems;
        }

    }
}