using BodyBuddy.Dtos;
using BodyBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly IQuoteService _quoteService;

        [ObservableProperty]
        public QuoteDto _quote;

        public MainPageViewModel(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        public async Task GetDailyQuote()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                Quote = await _quoteService.GetDailyQuote();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get daily quote {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
