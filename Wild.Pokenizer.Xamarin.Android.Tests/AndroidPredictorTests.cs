using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Wild.Pokenizer.Xamarin.Droid.Predictors;
using Xunit;

namespace Wild.Pokenizer.Xamarin.Android.Tests
{
    public class AndroidPredictorTests
    {
        [Fact]
        public void VersionCheckIsCorrect()
        {
            var sut = new DroidPredictor();
            sut.Version.Should().Be("1.0");
        }

        [Fact]
        public async Task DroidPredictorReturnsSuccessForStringInput()
        {
            var x = new DroidPredictor();
            var result = await x.PredictAsync("test");
            result.Answer.Should().BeEquivalentTo("Prediction from Android string.");
        }

        [Fact]
        public async Task DroidPredictorReturnsSuccessForStreamInput()
        {
            var sut = new DroidPredictor();
            var stream = new MemoryStream();
            var result = await sut.PredictAsync(stream);

            result.Success.Should().BeTrue();
            result.Answer.Should().BeEquivalentTo("Prediction from Android stream.");
        }
    }
}
