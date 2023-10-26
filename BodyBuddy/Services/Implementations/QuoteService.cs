using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var dailyQuote = await _quoteRepository.FetchNewQuote();
            return _mapper.MapToDto(dailyQuote);
        }
    }
}
