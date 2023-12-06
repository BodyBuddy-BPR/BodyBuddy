using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IQuoteService
    {
        //Converts Db --> Dto object
        Task<QuoteDto> GetDailyQuote();
    }
}
