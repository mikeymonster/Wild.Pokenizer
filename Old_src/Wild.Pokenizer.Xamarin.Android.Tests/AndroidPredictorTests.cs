using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Xamarin.Droid.Predictors;
using Xunit;

namespace Wild.Pokenizer.Xamarin.Android.Tests
{
    public class AndroidPredictorTests
    {
        //[Fact]
        //public void VersionCheckIsCorrect()
        //{
        //    var sut = new DroidPredictor();
        //    sut.Version.Should().Be("1.0");
        //}

        [Fact]
        public async Task DroidPredictorReturnsSuccessForStringInput()
        {
            var assetLoader = Substitute.For<IAssetLoader>();
            var sut = new DroidPredictor(assetLoader);
            var result = await sut.PredictAsync("test");
            result.Answer.Should().BeEquivalentTo("Prediction from Android string.");
        }

        [Fact]
        public async Task DroidPredictorReturnsSuccessForStreamInput()
        {
            var assetLoader = Substitute.For<IAssetLoader>();
            assetLoader.GetStream(Arg.Any<string>())
                .Returns(new MemoryStream());

            var sut = new DroidPredictor(assetLoader);
            var stream = new MemoryStream();
            var result = await sut.PredictAsync(stream);

            result.Success.Should().BeTrue();
            result.Answer.Should().BeEquivalentTo("Prediction from Android stream.");
        }
    }
}
