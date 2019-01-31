using System;
using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Predictors;
using Xunit;

namespace Wild.Pokenizer.Core.Tests
{
    [Trait("Category", "Predictors")]
    public class PredictorTests : IDisposable
    {
        [Fact]
        public async Task DefaultPredictorReturnsSuccessForStringInput()
        {
            var sut = new DefaultPredictor();
            var question = "Is it true?";
            var result = await sut.PredictAsync(question);

            Assert.True(result.Success);
            Assert.Equal("Prediction from string.", result.Answer);
        }

        [Fact]
        public async Task DefaultPredictorReturnsSuccessForStreamInput()
        {
            var sut = new DefaultPredictor();
            var stream = new MemoryStream();
            var result = await sut.PredictAsync(stream);

            Assert.True(result.Success);
            Assert.Equal("Prediction from stream.", result.Answer);
        }

        public void Dispose()
        {
        }
    }
}
