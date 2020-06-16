using FluentAssertions;
using Moq;
using Plugin.Media.Abstractions;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace Wild.Pokenizer.Core.Tests
{
    public class MainViewModelTests
    {
        //private readonly IDisplayAlertProvider _displayAlertProvider;
        private readonly IMedia _media;
        private readonly MediaFile _mediaFile;
        private readonly IMediaProvider _mediaProvider;
        private readonly IPredictor _predictor;

        private readonly ITestOutputHelper _output;

        public MainViewModelTests(ITestOutputHelper output)
        {
            _output = output;

            //    _predictor = Substitute.For<IPredictor>();
            //    _predictor
            //        .PredictAsync(Arg.Any<Stream>())
            //        .Returns(new PredictionResult
            //        {
            //            Success = true,
            //            Answer = "Test result",
            //            PredictionTime = new DateTime(2000, 1, 1)
            //        });

            ////_displayAlertProvider = Substitute.For<IDisplayAlertProvider>();

            //_mediaFile = new MediaFile(
            //    "/files/testfile",
            //    () => new MemoryStream(),
            //    () => new MemoryStream());

            //_media = Substitute.For<IMedia>();
            //_media.Initialize().Returns(Task.FromResult(true));
            //_media.IsPickPhotoSupported.Returns(true);
            //_media.IsTakePhotoSupported.Returns(true);
            //_media.IsCameraAvailable.Returns(true);
            //_media.PickPhotoAsync(Arg.Any<PickMediaOptions>()).Returns(Task.FromResult(_mediaFile));
            //_media.TakePhotoAsync(Arg.Any<StoreCameraMediaOptions>()).Returns(Task.FromResult(_mediaFile));

            //_mediaProvider = Substitute.For<IMediaProvider>();
            //_mediaProvider.Media.Returns(_media);
        }

        [Fact]
        public void MainViewModel_Constructor_Sets_Default_Properties()
        {
            var mockMediaProvider = new Mock<IMediaProvider>();
            var mockPredictor = Mock.Of<IPredictor>();
            var viewModel = new MainViewModel(mockMediaProvider.Object, mockPredictor);

            viewModel.Predictions.Should().NotBeNull();
            viewModel.Predictions.Should().BeEmpty();

            viewModel.TopPrediction.Should().BeNull();
        }

        [Fact]
        public void MainViewModel_Default_Message_Has_Expected_Value()
        {
            var mockMediaProvider = Mock.Of<IMediaProvider>();
            var mockPredictor = Mock.Of<IPredictor>();
            var viewModel = new MainViewModel(mockMediaProvider, mockPredictor);

            viewModel.Message.Should().BeEmpty();
        }
    }
}
