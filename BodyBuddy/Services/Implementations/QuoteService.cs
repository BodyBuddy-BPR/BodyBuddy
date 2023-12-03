using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models;
using BodyBuddy.Helpers;

namespace BodyBuddy.Services.Implementations
{
    public class QuoteService : IQuoteService
    {
        private IQuoteRepository _quoteRepository;
        private QuoteMapper _mapper;

        public QuoteService(IQuoteRepository quoteRepository, QuoteMapper quoteMapper)
        {
            _quoteRepository = quoteRepository;
            _mapper = quoteMapper;
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
