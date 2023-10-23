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

        public QuoteRepository(DateTimeService dateTimeService, Client supabaseClient)
        {
            _dateTimeService = dateTimeService;
            _supabaseClient = supabaseClient;
        }

        public async Task<Quote> FetchNewQuote()
        {
            DateTime lastFetchedDate = Preferences.Get(datePreferencesKey, DateTime.MinValue);

            // Check if new day using the DateTimeService
            if (_dateTimeService.IsNewDay(lastFetchedDate))
            {

                using (var httpClient = new HttpClient())
                {
                    string apiUrl = $"{AppSettingKeys.SUPABASE_URL}/rest/v1/random_quote?apikey={AppSettingKeys.SUPABASE_KEY}&limit=1";
                    var response = await httpClient.GetStringAsync(apiUrl);

                    // Deserialize the JSON response
                    var quote = JsonConvert.DeserializeObject<Quote[]>(response)[0];

                    // Save the new quote and update the last fetched date in SharedPreferences
                    Preferences.Set(datePreferencesKey, _dateTimeService.Today);
                    Preferences.Set(quotePreferencesKey, quote.quote);

                    return quote;
                }
            }

            // Return the previously fetched quote
            return new Quote { quote = Preferences.Get(quotePreferencesKey, "To enjoy the glow of good health, you must exercise."), Author = "Gene Tunney" };
        }
    }
}
