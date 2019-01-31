using NSubstitute;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.ViewModels;
using Xunit;

namespace Wild.Pokenizer.Core.Tests
{
    [Trait("Category", "View Modeld")]
    public class MainViewModelTests
    {
        private IMediaProvider _mediaProvider;
        private IPredictor _predictor;

        public MainViewModelTests()
        {
            _predictor = Substitute.For<IPredictor>();
            _mediaProvider = Substitute.For<IMediaProvider>();
        }

        [Fact(DisplayName = "Take Picture command is initialised")]
        public void MainViewModelTakePictureCommandIsInitialised()
        {
            var sut = new MainViewModel(_mediaProvider, _predictor);
            Assert.NotNull(sut.TakePictureCommand);
        }

        [Fact(DisplayName = "Upload Picture command is initialised")]
        public void MainViewModelUploadPictureCommandInitialised()
        {
            var sut = new MainViewModel(_mediaProvider, _predictor);
            Assert.NotNull(sut.UploadPictureCommand);
        }

        [Fact(DisplayName = "Predict command is initialised")]
        public void MainViewModePredictCommandIsInitialised()
        {
            var sut = new MainViewModel(_mediaProvider, _predictor);
            Assert.NotNull(sut.PredictCommand);
        }
    }
}
