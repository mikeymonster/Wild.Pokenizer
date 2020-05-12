using Plugin.Media;
using Plugin.Media.Abstractions;
using Wild.Pokenizer.Core.Interfaces;

namespace Wild.Pokenizer.Services
{
    public class MediaProvider : IMediaProvider
    {
        public IMedia Media => CrossMedia.Current;
    }
}
