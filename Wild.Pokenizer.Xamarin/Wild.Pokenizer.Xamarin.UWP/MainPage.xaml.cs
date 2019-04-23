
namespace Wild.Pokenizer.Xamarin.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new Xamarin.App(new IoC.Setup()));
        }
    }
}
