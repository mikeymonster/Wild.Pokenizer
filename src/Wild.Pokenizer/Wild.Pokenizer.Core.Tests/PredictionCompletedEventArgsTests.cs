using System.Collections.Generic;
using FluentAssertions;
using Wild.Pokenizer.Core.Models;
using Xunit;

namespace Wild.Pokenizer.Core.Tests
{
    public class PredictionCompletedEventArgsTests
    {
        [Fact]
        public void PredictionCompletedEventArgs_Constructor_Sets_Properties()
        {
            var predictions = new List<Prediction>
            {
                new Prediction("Rain", .98f)
            };

            var args = new PredictionCompletedEventArgs(predictions);

            args.Predictions.Should().BeEquivalentTo(predictions);
        }
    }
}
