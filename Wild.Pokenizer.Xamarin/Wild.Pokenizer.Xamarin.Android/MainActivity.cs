using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Autofac;
using Plugin.CurrentActivity;
using Plugin.Permissions;

namespace Wild.Pokenizer.Xamarin.Droid
{
    [Activity(Label = "Wild.Pokenizer.Xamarin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //CrossCurrentActivity.Current.Init(this, bundle);

            //CheckPermissions();
            //if (!Resolver.IsSet) SetIoc();

            LoadApplication(new App());
        }

        private void CheckPermissions()
        {
            var cameraPermission = Android.Manifest.Permission.Camera;

            if (ContextCompat.CheckSelfPermission(this, cameraPermission) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, 
                    new string[]
                    {
                        cameraPermission
                    },
                    (int)Permission.Granted);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetIoc()
        {
            //var containerBuilder = new Autofac.ContainerBuilder();

            //containerBuilder.Register(c => AndroidDevice.CurrentDevice).As<IDevice>();
            //containerBuilder.RegisterType<MainViewModel>().AsSelf();

            //Resolver.SetResolver(new AutofacResolver(containerBuilder.Build()));

        }
    }
}