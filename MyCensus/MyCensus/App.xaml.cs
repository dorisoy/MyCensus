
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Prism.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Unity;

using MyCensus.ViewModels;
using MyCensus.Views;
using MyCensus.Services;
using MyCensus.DataServices;
using MyCensus.Controls;
using MyCensus.ViewModels.Base;
using MyCensus.Utils;

//using FormsPlugin.Iconize;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyCensus
{
    public partial class App : PrismApplication
    {
        //private readonly IContainerProvider _unityContainer;

        public static IContainerProvider ContainerProvider;

        public static int IndexTab { get; set; }

        public static INavigationService navigationService;

        /* 
        * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
        * This imposes a limitation in which the App class must have a default constructor. 
        * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
        */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
            ContainerProvider = Container;
            navigationService = NavigationService;
        }

        public static T ResolveService<T>()
        {
            var service = ContainerProvider.Resolve<T>();
            return service;
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            //初始DataGrid 组件
            Xamarin.Forms.DataGrid.DataGridComponent.Init();

            //MyCensus.Utils
            //Task.Factory.StartNew(() => new MainPage()).LogExceptions();
            //Task.Factory.StartNew(() => new MyTask()).LogExceptions();


            //base PrismApplication
            var _authenticationService = Container.Resolve<IAuthenticationService>();
            //DependencyResolver.Container.Resolve<ILogger>()
            //如果认证登录
            if (_authenticationService.IsAuthenticated)
            {
                //已经登录 using FormsPlugin.Iconize; IconNavigationPage
                //await NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
                await NavigationService.NavigateAsync("CustomNavigationPage/MainPage");
                //Current.MainPage = new CustomNavigationPage(new MyCensus.Views.MainPage());
            }
            else
            {
                //var pages = Current.MainPage.Navigation.NavigationStack;
                await NavigationService.NavigateAsync("LoginPage?title=LoginPage");
                //await NavigationService.NavigateAsync("MainPage");
            }
        }


        //public static MainPage RootPage()
        //{
        //    return (MainPage)Current.MainPage;
        //}

        //public static void NavigateToHomePage()
        //{
        //    try
        //    {
        //        MainPage homePage = new MainPage();
        //        MyMasterDetail masterDetailRootPage = (MyMasterDetail)Application.Current.MainPage;
        //        masterDetailRootPage.Detail = new NavigationPage(homePage);
        //        masterDetailRootPage.IsPresented = false;

        //        Current.MainPage = masterDetailRootPage;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("!!! NavigateToHomePage() Exception !!!");
        //        Debug.WriteLine("Exception Description: " + ex);
        //    }
        //}


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
           // containerRegistry.RegisterForNavigation<NavigationPage>();
            //containerRegistry.RegisterForNavigation<IconNavigationPage>();
            containerRegistry.RegisterForNavigation<CustomNavigationPage>();

            containerRegistry.RegisterForNavigation<MainPage>();

            //Page
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<SiteMap, SiteMapViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
            containerRegistry.RegisterForNavigation<MenuPage, MenuPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<ReportIncidentPage, ReportIncidentPageViewModel>();

            containerRegistry.RegisterForNavigation<MyTask, MyTaskViewModel>();
            containerRegistry.RegisterForNavigation<Restaurant_Card_Page1, Restaurant_Card_Page1ViewModel>();
            containerRegistry.RegisterForNavigation<Restaurant_Card_Page2, Restaurant_Card_Page2ViewModel>();
            containerRegistry.RegisterForNavigation<Restaurant_Card_Page3, Restaurant_Card_Page3ViewModel>();

            containerRegistry.RegisterForNavigation<Tradition_Card_Page1, Tradition_Card_Page1ViewModel>();
            containerRegistry.RegisterForNavigation<Tradition_Card_Page2, Tradition_Card_Page2ViewModel>();
            containerRegistry.RegisterForNavigation<Tradition_Card_Page3, Tradition_Card_Page3ViewModel>();

            containerRegistry.RegisterForNavigation<Networks, NetworksViewModel>();
            containerRegistry.RegisterForNavigation<TraditionViewCard, TraditionViewCardViewModel>();
            containerRegistry.RegisterForNavigation<RestaurantViewCard, RestaurantViewCardViewModel>();
            containerRegistry.RegisterForNavigation<DialogKitTest, DialogKitTestViewModel>();
            containerRegistry.RegisterForNavigation<AddProducts, AddProductsViewModel>();
            containerRegistry.RegisterForNavigation<ScanPage, ScanPageViewModel>();
            containerRegistry.RegisterForNavigation<SaoPage, SaoPageViewModel>();
            containerRegistry.RegisterForNavigation<TimeLinePage, TimeLinePageViewModel>();
            containerRegistry.RegisterForNavigation<UpdatePage, UpdatePageViewModel>();
            containerRegistry.RegisterForNavigation<ViewLocation>();
            containerRegistry.RegisterForNavigation<SearchPage>();
        }



        //public App ()
        //{
        //	InitializeComponent();
        //  tabs = new BottomTabbedPage();
        //  SetMainPage();
        //}

        /* 
     * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
     * This imposes a limitation in which the App class must have a default constructor. 
     * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
     */
        //public App() : this(null) { }

        //public App(IPlatformInitializer initializer) : base(initializer) {

        //}

        /*

    public App()
    {
        InitializeComponent();

        //NavigationPage = new CustomNavigationPage(new MyCensus.Views.MainPage());

        请考虑使用 app.config 将程序集“Unity.ServiceLocation, Culture=neutral, PublicKeyToken=489b6accfaf20ef0”从版本“2.1.1.0”
        [C:\Users\Administrator\.nuget\packages\unity.servicelocation\2.1.1\lib\netstandard2.0\Unity.ServiceLocation.dll]重新映射到版本“2.1.2.0”
        [C:\Users\Administrator\.nuget\packages\unity\5.8.6\lib\netstandard2.0\Unity.ServiceLocation.dll]，以解决冲突并消除警告。

        //Load the assembly
        Xamarin.Forms.DataGrid.DataGridComponent.Init();

        AdaptColorsToHexString();

        if (Device.RuntimePlatform == Device.UWP)
        {

            // return new NavigationPage(new SplashscreenPage());
            InitNavigation();
        }
    }


    protected override async void OnStart()
    {
        base.OnStart();
        if (Device.RuntimePlatform != Device.UWP)
        {
            await InitNavigation();

            //var navigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
         //
            Current.MainPage = new CustomNavigationPage(new MyCensus.Views.MainPage());
        }
    }

    private Task InitNavigation()
    {

        //IOC
        var navigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
        return navigationService.InitializeAsync();
    }


    private void AdaptColorsToHexString()
    {
        for (var i = 0; i < this.Resources.Count; i++)
        {
            var key = this.Resources.Keys.ElementAt(i);
            var resource = this.Resources[key];

            if (resource is Color color)
            {
                this.Resources.Add(key + "HexString", color.ToHexString());
            }
        }
    }

    protected override void OnSleep()
    {
        // Handle when your app sleeps
    }

    protected override void OnResume()
    {
        // Handle when your app resumes
    }

    //protected override async void OnInitialized()
    //{
    //    InitializeComponent();

    //    //NavigationPage = new NavigationPageGradientHeader(Current.MainPage);
    //    //MainPage = new Controls.NavigationPageGradientHeader(Current.MainPage)
    //    //{
    //    //    LeftColor = Color.FromHex("#36ED81"),
    //    //    RightColor = Color.FromHex("#109F8D")
    //    //};
    //    //MainPage = new CustomNavigationPage(new MyCensus.Views.MainPage());


    //    //Load the assembly
    //    Xamarin.Forms.DataGrid.DataGridComponent.Init();


    //    //await NavigationService.NavigateAsync("NavigationPage/MainPage");
    //    //bool isLogin = false;//这里只是一个假设值，真实项目中，应该判断是否已登录。
    //    //if (isLogin)
    //    //{
    //    //    //已经登录
    //    //    //await NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
    //    //    await NavigationService.NavigateAsync("SiteMap?title=SiteMap");
    //    //}
    //    //else
    //    //{
    //    //    await NavigationService.NavigateAsync("LoginPage?title=LoginPage");
    //    //}

    //}


    //protected override void RegisterTypes(IContainerRegistry containerRegistry)
    //{
    //    //CustomNavigationPage 
    //    //containerRegistry.RegisterForNavigation<NavigationPage>();

    //    //containerRegistry.RegisterForNavigation<CustomNavigationPage>();
    //    //containerRegistry.RegisterForNavigation<MainPage>();
    //    //containerRegistry.RegisterForNavigation<LoginPage>();
    //    //containerRegistry.RegisterForNavigation<PrismTabbedPage1>();
    //    //containerRegistry.RegisterForNavigation<SiteMap>();
    //    //containerRegistry.RegisterForNavigation<AboutPage>();

    //    //containerRegistry.RegisterForNavigation<MyTask>();
    //    //containerRegistry.RegisterForNavigation<Tradition_Card>();
    //    //containerRegistry.RegisterForNavigation<Restaurant_Card>();
    //    //containerRegistry.RegisterForNavigation<Restaurant_Card_Page1>();
    //    //containerRegistry.RegisterForNavigation<Restaurant_Card_Page2>();
    //    //containerRegistry.RegisterForNavigation<Restaurant_Card_Page3>();
    //    //containerRegistry.RegisterForNavigation<Tradition_Card_Page1>();
    //    //containerRegistry.RegisterForNavigation<Tradition_Card_Page2>();
    //    //containerRegistry.RegisterForNavigation<Tradition_Card_Page3>();
    //    //containerRegistry.RegisterForNavigation<Networks>();
    //    //containerRegistry.RegisterForNavigation<ViewCard>();
    //}






    //public static void SetMainPage()
    //{

    //    //switch (Device.RuntimePlatform)
    //    //{
    //    //    case Device.iOS:
    //    //        top = 20;
    //    //        break;
    //    //    case Device.Android:
    //    //    case Device.UWP:
    //    //    default:
    //    //        top = 0;
    //    //        break;
    //    //}

    //    //Current.MainPage = new MasterDetailPage
    //    //{
    //    //    Master = new NavigationPage(new UserPage())
    //    //    {
    //    //        Title = "用户信息",
    //    //        Icon = "tab_about.png"
    //    //    },
    //    //    Detail = new NavigationPage(new MainPage())
    //    //    {
    //    //        Title = "列表",
    //    //        Icon= "tab_feed.png"
    //    //    }
    //    //};

    //    //var icons = Plugin.Iconize.Iconize.Modules.FirstOrDefault()?.Keys.Take(5) ?? new string[0];
    //    //new NavigationPage(new Page1() { Title = "位置", Icon = "fa-flag-checkered" }) { BarBackgroundColor = Color.Green, BarTextColor = Color.Blue }

    //    //tabs.CurrentPageChanged += Tabs_CurrentPageChanged;
    //    //tabs.PagesChanged += Tabs_PagesChanged;
    //    //tabs.ChildrenReordered += Tabs_ChildrenReordered;


    //    //tabs.Children.Add(new SiteMap()
    //    //{
    //    //    Title = "位置",
    //    //    Icon = "fa-flag-checkered"
    //    //});

    //    //tabs.Children.Add(new MyTask()
    //    //{
    //    //    Title = "任务",
    //    //    Icon = "fa-windows"
    //    //});

    //    //tabs.Children.Add(new Networks()
    //    //{
    //    //    Title = "网点",
    //    //    Icon = "fa-truck"
    //    //});

    //    //tabs.Children.Add(new AboutPage()
    //    //{
    //    //    Title = "我",
    //    //    Icon = "fa-user"
    //    //});


    //    ////

    //    //var myNavigationPage = new NavigationPage(tabs);
    //    //myNavigationPage.Title = "首页";
    //    //myNavigationPage.BackgroundColor = Color.DodgerBlue;
    //    //myNavigationPage.BackgroundColor = Color.White;

    //    //Current.MainPage = new MasterDetailPage
    //    //{
    //    //    Master = new NavigationPage(new UserPage())
    //    //    {
    //    //        Title = "用户信息",
    //    //        Icon = "tab_about.png"
    //    //    },
    //    //    Detail = myNavigationPage
    //    //};
    //    //layout.Margin = new Thickness(5, top, 5, 0);

    //    //Current.MainPage = myNavigationPage;
    //}


    */


    }
}
