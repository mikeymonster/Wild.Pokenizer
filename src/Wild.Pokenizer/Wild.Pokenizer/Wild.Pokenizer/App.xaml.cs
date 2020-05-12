using Wild.Pokenizer.Services;
using Wild.Pokenizer.Views;

namespace Wild.Pokenizer
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            Bootstrapper.Init(this);

            MainPage = new MainShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
