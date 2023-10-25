using BodyBuddy.Dtos;
using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
