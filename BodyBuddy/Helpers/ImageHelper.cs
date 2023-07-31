using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Helpers
{
    public class ImageHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagesString)
            {
                var imagePaths = imagesString.Split(',').Select(path => path.Trim()).ToList();
                if (imagePaths.Count > 0)
                {
                    return new Uri(imagePaths[0]);
                }
            }

            // Return a default image if the Images string is empty or null
            return new Uri("no_image.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
