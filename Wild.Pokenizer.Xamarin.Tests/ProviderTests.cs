using System;
using Xunit;

namespace Wild.Pokenizer.Xamarin.Tests
{
    [Trait("Category", "Providers")]
    public class ProviderTests : IDisposable
    {
        //[Fact(DisplayName = "Returns current CrossMedia instance")]
        ////[Fact(DisplayName = "Returns current CrossMedia instance", 
        ////    Skip = "Cannot test - needs platform-specific implementation")]
        //public void MediaProviderReturnsCurrentCrossMediaInstance()
        //{
        //    var sut = new MediaProvider();

        //    Assert.NotNull(sut.Media);
        //    //Assert.Equal(CrossMedia.Current, sut.Media);
        //}

        public void Dispose()
        {
        }
    }
}
