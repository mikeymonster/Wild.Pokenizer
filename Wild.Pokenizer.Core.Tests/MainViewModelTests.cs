using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel.Resolution;
using NSubstitute;
using Wild.Pokenizer.Core.ViewModels;
using Xunit;

namespace Wild.Pokenizer.Core.Tests
{
    public class MainViewModelTests
    {
        [Fact]
        public void MainViewModelTakePictureCommandIsInitialised()
        {
            var sut = new MainViewModel();
            Assert.NotNull(sut.TakePictureCommand);
        }

        [Fact]
        public void MainViewModelUploadPictureCommandsInitialised()
        {
            var sut = new MainViewModel();
            Assert.NotNull(sut.UploadPictureCommand);
        }

        [Fact]
        public void MainViewModePredictCommandIsInitialised()
        {
            var sut = new MainViewModel();
            Assert.NotNull(sut.PredictCommand);
        }
    }
}
