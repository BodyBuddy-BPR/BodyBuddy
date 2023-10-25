using BodyBuddy.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services
{
    public interface IQuoteService
    {
        //Converts Db --> Dto object
        Task<QuoteDto> GetDailyQuote();
    }
}
