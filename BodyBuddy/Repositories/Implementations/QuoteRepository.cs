using BodyBuddy.Database;
using BodyBuddy.Models;
using Newtonsoft.Json;
using Supabase;

namespace BodyBuddy.Repositories.Implementations
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly Client _supabaseClient;

        private readonly string quotePreferencesKey = "LastFetchedQuote";
        private readonly string authorPreferencesKey = "LastFetchedAuthor";

#if GITHUB_BUILD
    string url = Environment.GetEnvironmentVariable("SUPABASE_URL");
    string key = Environment.GetEnvironmentVariable("SUPABASE_KEY");
#else
        string url = AppSettingKeys.SUPABASE_URL;
        string key = AppSettingKeys.SUPABASE_KEY;
#endif

        public QuoteRepository(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<QuoteModel> FetchNewQuote()
        {
            using var httpClient = new HttpClient();
            var apiUrl = $"{url}/rest/v1/random_quote?apikey={key}&limit=1";
            var response = await httpClient.GetStringAsync(apiUrl);

            // Deserialize the JSON response
            var quote = JsonConvert.DeserializeObject<QuoteModel[]>(response)[0];

            // Save the new quote and update the last fetched date in SharedPreferences
            Preferences.Set(quotePreferencesKey, quote.Quote);
            Preferences.Set(authorPreferencesKey, quote.Author);

            return quote;
        }

        public QuoteModel GetSavedQuote()
        {
            var savedQuote = new QuoteModel
            {
                Quote = Preferences.Get(quotePreferencesKey, "To enjoy the glow of good health, you must exercise."),
                Author = Preferences.Get(authorPreferencesKey, "Gene Tunney")
            };

            return savedQuote;
        }
    }
}
