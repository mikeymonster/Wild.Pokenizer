using Plugin.Media.Abstractions;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IMediaProvider
    {
        IMedia Media { get; }
    }
}
