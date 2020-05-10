using System;
using System.Reflection;
using Autofac;
using TinyCacheLib;
using TinyCacheLib.FileStorage;
using TinyMvvm.Autofac;
using TinyMvvm.IoC;
using TinyNavigationHelper;
using TinyNavigationHelper.Forms;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.ViewModels;
using Xamarin.Forms;

namespace Wild.Pokenizer
{
    public class Bootstrapper
    {
        public static IBootstrapper Platform { get; set; }

        public static void Init(Application app)
        {
            var builder = new ContainerBuilder();

            Platform?.Init(builder);

            var navigation = new FormsNavigationHelper();
            navigation.RegisterViewsInAssembly(Assembly.GetExecutingAssembly());

            builder.RegisterType<Views.MainView>();

            builder.RegisterType<FormsNavigationHelper>().As<INavigationHelper>();

            builder.RegisterType<MainViewModel>();

            //builder.RegisterType<DefaultPredictor>().As<IPredictor>();

            var container = builder.Build();

            Resolver.SetResolver(new AutofacResolver(container));

            TinyMvvm.Forms.TinyMvvm.Initialize();

            var cache = TinyCacheHandler.Create("FileCache");

            var fileStorage = new FileStorage();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            fileStorage.Initialize(path);

            cache.SetCacheStore(fileStorage);
        }
    }
}
