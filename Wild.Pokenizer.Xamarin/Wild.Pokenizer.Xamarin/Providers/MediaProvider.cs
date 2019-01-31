using Plugin.Media;
using Plugin.Media.Abstractions;
using Wild.Pokenizer.Core.Interfaces;

namespace Wild.Pokenizer.Xamarin.Providers
{
    public class MediaProvider : IMediaProvider
    {
        public IMedia Media => CrossMedia.Current;
    }
}
