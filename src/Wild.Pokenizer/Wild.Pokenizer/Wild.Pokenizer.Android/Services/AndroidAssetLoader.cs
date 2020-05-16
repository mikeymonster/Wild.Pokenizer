using System.IO;
using Android.App;
using Wild.Pokenizer.Core.Interfaces;

namespace Wild.Pokenizer.Droid.Services
{
    public class AndroidAssetLoader : IAssetLoader
    {
        public Stream GetStream(string path)
        {
            var assetManager = Application.Context.Assets;
            return assetManager.Open(path);
        }
    }
}