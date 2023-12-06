using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IQuoteRepository
    {
        Task<QuoteModel> FetchNewQuote();

        QuoteModel GetSavedQuote();
    }
}
