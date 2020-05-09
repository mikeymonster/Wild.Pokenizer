using System.IO;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IAssetLoader
    {
        /// <summary>
        /// Get a stream for loading an asset from a path.
        /// </summary>
        Stream GetStream(string path);
    }
}
