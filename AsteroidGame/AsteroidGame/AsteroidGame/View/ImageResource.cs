using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AsteroidGame.View
{
    [ContentProperty(nameof(Source))]
    public class ImageResource : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null) return null;
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResource).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}