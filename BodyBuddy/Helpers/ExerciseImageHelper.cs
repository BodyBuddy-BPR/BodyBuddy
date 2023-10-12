using System.Globalization;

namespace BodyBuddy.Helpers
{
    public class ExerciseImageHelper : IValueConverter
    {
        private const string BaseImageUrl = "https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/exercises/";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imageString)
            {
                var imagePaths = imageString.Split(',').Select(path => path.Trim()).ToList();

                var imagePath = BaseImageUrl + imagePaths[0];

                var imageUrl = BaseImageUrl + imagePaths[0];

                return new Uri(imageUrl);
            }
            return new Uri("no_image.png") ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
