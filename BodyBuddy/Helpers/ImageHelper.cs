using Imagekit;
using Imagekit.Sdk;
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
        // TODO: FIGURE OUT HOW TO HIDE THESE KEYS
        private const string PrivateApiKey = "private_YWs786SoD61eD9kxM/Q2sktOfs0=";
        private const string PublicApiKey = "public_tIwRm05NHB+Qaaw9+2yv1Ku2Oq0=";
        private const string ImagekitEndpoint = "https://ik.imagekit.io/puguoz8sl"; // The Imagekit endpoint URL

        private const string BaseImageUrl = "https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/exercises/";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagesString)
            {
                var imagePaths = imagesString.Split(',').Select(path => path.Trim()).ToList();
                if (imagePaths.Count > 0)
                {
                    // Get the first image path
                    var imagePath = BaseImageUrl + imagePaths[0];

                    // Use Imagekit to generate the URL with transformations
                    //var imageUrl = GenerateImagekitURL(imagePath);

                    var imageUrl = BaseImageUrl + imagePaths[0];

                    return new UriImageSource { Uri = new Uri(imageUrl) };
                }
            }

            // Return a default image if the Images string is empty or null
            return new UriImageSource { Uri = new Uri("no_image.png") };
        }

        // Using ImageKit to provide optimized and resized images for faster loading
        private string GenerateImagekitURL(string imagePath)
        {
            // Initialize the Imagekit client
            ImagekitClient imagekit = new ImagekitClient(PublicApiKey, PrivateApiKey, ImagekitEndpoint);

            // Sample transformation settings
            Transformation transformation = new Transformation()
                //.Height(160)
                //.Width(160)
                .Progressive(true)
                .Quality(10);

            // Generate the Imagekit URL with transformations
            string imageURL = imagekit.Url(transformation).Path(imagePath).Generate();

            return imageURL;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
