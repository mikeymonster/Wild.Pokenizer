using System.IO;
using Android.App;
using Wild.Pokenizer.Core.Interfaces;

namespace Wild.Pokenizer.Xamarin.Droid
{
    public class DroidAssetLoader : IAssetLoader
    {
        public Stream GetStream(string path)
        {
            var assetManager = Application.Context.Assets;
            return assetManager.Open(path);
        }
    }
}