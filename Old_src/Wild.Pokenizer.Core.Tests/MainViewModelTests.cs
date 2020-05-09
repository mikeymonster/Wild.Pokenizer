using System;
using System.IO;
using System.Threading.Tasks;
using NSubstitute;
using Plugin.Media.Abstractions;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;
using Wild.Pokenizer.Core.ViewModels;
using Xunit;

namespace Wild.Pokenizer.Core.Tests
{
    [Trait("Category", "View Model")]
    public class MainViewModelTests
    {
        private readonly IDisplayAlertProvider _displayAlertProvider;
        private readonly IMedia _media;
        private readonly MediaFile _mediaFile;
        private readonly IMediaProvider _mediaProvider;
        private readonly IPredictor _predictor;

        public MainViewModelTests()
        {
            _predictor = Substitute.For<IPredictor>();
            _predictor
                .PredictAsync(Arg.Any<Stream>())
                .Returns(new PredictionResult
                {
                    Success = true,
                    Answer = "Test result",
                    PredictionTime = new DateTime(2000, 1, 1)
                });

            _displayAlertProvider = Substitute.For<IDisplayAlertProvider>();

            _mediaFile = new MediaFile(
                "/files/testfile",
                () => new MemoryStream(),
                () => new MemoryStream());

            _media = Substitute.For<IMedia>();
            _media.Initialize().Returns(Task.FromResult(true));
            _media.IsPickPhotoSupported.Returns(true);
            _media.IsTakePhotoSupported.Returns(true);
            _media.IsCameraAvailable.Returns(true);
            _media.PickPhotoAsync(Arg.Any<PickMediaOptions>()).Returns(Task.FromResult(_mediaFile));
            _media.TakePhotoAsync(Arg.Any<StoreCameraMediaOptions>()).Returns(Task.FromResult(_mediaFile));

            _mediaProvider = Substitute.For<IMediaProvider>();
            _mediaProvider.Media.Returns(_media);
        }

        [Fact(DisplayName = "Take Picture command is initialised")]
        public void MainViewModelTakePictureCommandIsInitialised()
        {
            var sut = new MainViewModel(_displayAlertProvider, _mediaProvider, _predictor);
            Assert.NotNull(sut.TakePictureCommand);
        }

        [Fact(DisplayName = "Upload Picture command is initialised")]
        public void MainViewModelUploadPictureCommandInitialised()
        {
            var sut = new MainViewModel(_displayAlertProvider, _mediaProvider, _predictor);
            Assert.NotNull(sut.UploadPictureCommand);
        }
    }
}
