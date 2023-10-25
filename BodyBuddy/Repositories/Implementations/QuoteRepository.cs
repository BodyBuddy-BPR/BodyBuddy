using BodyBuddy.Database;
using BodyBuddy.Models;
using BodyBuddy.Services;
using Newtonsoft.Json;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories.Implementations
{
    public class QuoteRepository : IQuoteRepository
    {
        private DateTimeService _dateTimeService;
        private readonly Client _supabaseClient;

        private readonly string datePreferencesKey = "LastFetchedDate";
        private readonly string quotePreferencesKey = "LastFetchedQuote";

#if GITHUB_BUILD
    string url = Environment.GetEnvironmentVariable("SUPABASE_URL");
    string key = Environment.GetEnvironmentVariable("SUPABASE_KEY");
#else
        string url = AppSettingKeys.SUPABASE_URL;
        string key = AppSettingKeys.SUPABASE_KEY;
#endif

        public QuoteRepository(DateTimeService dateTimeService, Client supabaseClient)
        {
            _dateTimeService = dateTimeService;
            _supabaseClient = supabaseClient;
        }

        public async Task<QuoteModel> FetchNewQuote()
        {
            DateTime lastFetchedDate = Preferences.Get(datePreferencesKey, DateTime.MinValue);

            // Check if new day using the DateTimeService
            if (_dateTimeService.IsNewDay(lastFetchedDate))
            {

                using (var httpClient = new HttpClient())
                {
                    string apiUrl = $"{url}/rest/v1/random_quote?apikey={key}&limit=1";
                    var response = await httpClient.GetStringAsync(apiUrl);

                    // Deserialize the JSON response
                    var quote = JsonConvert.DeserializeObject<QuoteModel[]>(response)[0];

                    // Save the new quote and update the last fetched date in SharedPreferences
                    Preferences.Set(datePreferencesKey, _dateTimeService.Today);
                    Preferences.Set(quotePreferencesKey, quote.Quote);

                    return quote;
                }
            }

            // Return the previously fetched quote
            return new QuoteModel { Quote = Preferences.Get(quotePreferencesKey, "To enjoy the glow of good health, you must exercise."), Author = "Gene Tunney" };
        }
    }
}
