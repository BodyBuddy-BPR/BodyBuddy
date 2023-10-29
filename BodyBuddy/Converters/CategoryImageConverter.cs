using System.Globalization;

namespace BodyBuddy.Converters
{
    public class CategoryImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string categoryName)
            {
                string modifiedCategoryName = categoryName.ToLower().Replace(" ", "_");
                return $"Categories/{modifiedCategoryName}.png";
            }
            return "no_image.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
