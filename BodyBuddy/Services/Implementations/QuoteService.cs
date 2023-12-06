using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Models;
using BodyBuddy.Helpers;

namespace BodyBuddy.Services.Implementations
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly QuoteMapper _mapper = new();

        public QuoteService(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public async Task<QuoteDto> GetDailyQuote()
        {
            QuoteModel quote;

            if (DateHelper.IsNewDay())
            {
                quote = await _quoteRepository.FetchNewQuote();
            }
            else
            {
               quote = _quoteRepository.GetSavedQuote();
            }

            return _mapper.MapToDto(quote);
        }
    }
}
