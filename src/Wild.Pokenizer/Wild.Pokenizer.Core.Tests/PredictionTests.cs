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
            var sut = new Prediction();

            sut.Label.Should().BeNull();
            sut.Probability.Should().Be(0);
        }

        [Fact]
        public void Prediction_Constructor_Sets_Properties()
        {
            var sut = new Prediction("Test_Label", 0.95f);

            sut.Label.Should().Be("Test_Label");
            sut.Probability.Should().BeApproximately(0.95f, 0.0001f);
        }
    }
}
