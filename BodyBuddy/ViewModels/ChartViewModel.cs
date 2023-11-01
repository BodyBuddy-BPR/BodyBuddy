using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;

namespace BodyBuddy.ViewModels
{
    public class ChartViewModel
    {
        public ObservableCollection<ChartData> Data { get; set; }

        public ChartViewModel()
        {
            Data = new ObservableCollection<ChartData>()
            {
                new ChartData { XValue = new DateTime(2023, 01, 01), YValue = 30 },
                new ChartData { XValue = new DateTime(2023, 02, 01), YValue = 28 },
                new ChartData { XValue = new DateTime(2023, 03, 01), YValue = 34 },
                // Add more data as needed
            };
        }
    }
}
