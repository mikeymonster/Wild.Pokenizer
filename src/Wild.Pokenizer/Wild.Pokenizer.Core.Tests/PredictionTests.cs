using FluentAssertions;
using Wild.Pokenizer.Core.Models;
using Xunit;

namespace Wild.Pokenizer.Core.Tests
{
    public class PredictionTests
    {
        [Fact]
        public void Prediction_Default_Constructor_Sets_Default_Properties()
        {
            var prediction = new Prediction();

            prediction.Label.Should().BeNull();
            prediction.Probability.Should().Be(0);
        }

        [Fact]
        public void Prediction_Constructor_Sets_Properties()
        {
            var prediction = new Prediction("Test_Label", 0.95f);

            prediction.Label.Should().Be("Test_Label");
            prediction.Probability.Should().BeApproximately(0.95f, 0.0001f);
        }
    }
}
