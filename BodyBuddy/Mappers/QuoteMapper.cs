using BodyBuddy.Dtos;
using BodyBuddy.Models;

namespace BodyBuddy.Mappers
{
    public class QuoteMapper
    {
        public QuoteDto MapToDto(QuoteModel quote)
        {
            if (quote == null)
                return new QuoteDto();

            return new QuoteDto()
            {
                Id = quote.Id,
                Quote = quote.Quote,
                Author = quote.Author,
            };
        }

    }
}
