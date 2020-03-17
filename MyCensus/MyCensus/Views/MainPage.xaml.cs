using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Naxam.Controls.Forms;
using MyCensus.Controls;
using MyCensus.ViewModels;
using MyCensus.ViewModels.Base;

namespace MyCensus.Views
{
	public partial class MainPage : BottomTabbedPage
    {
        public MainPage()
        {
            InitializeComponent ();

            /*
             Unity.Exceptions.ResolutionFailedException: Resolution of the dependency failed, type = 'System.Object', name = 'MainPage'.
            Exception occurred while: Calling constructor MyCensus.Views.MainPage().
            Exception is: ArgumentNullException - Value cannot be null.
            Parameter name: activated
            -----------------------------------------------
            At the time of the exception, the container was: 
              Resolving MyCensus.Views.MainPage,MainPage (mapped from System.Object, MainPage)
              Calling constructor MyCensus.Views.MainPage()


            Unhandled Exception:

            Unity.Exceptions.ResolutionFailedException: Resolution of the dependency failed, type = 'System.Object', name = 'MainPage'.
            Exception occurred while: Calling constructor MyCensus.Views.MainPage().
            Exception is: ArgumentNullException - Value cannot be null.
            Parameter name: activated
            -----------------------------------------------
            At the time of the exception, the container was: 
              Resolving MyCensus.Views.MainPage,MainPage (mapped from System.Object, MainPage)
              Calling constructor MyCensus.Views.MainPage()
             出现了 
             */

            try
            {
                CustomNavigationPage.SetTitleIcon(this, "icon.png");
                CustomNavigationPage.SetHasNavigationBar(this, true);
                CustomNavigationPage.SetHasBackButton(this, false);

                CustomNavigationPage.SetTitleMargin(this, new Thickness(0, 0, 5, 0));
                CustomNavigationPage.SetTitleColor(this, Color.White);
                CustomNavigationPage.SetSubtitleColor(this, Color.White);
                CustomNavigationPage.SetTitlePosition(this, CustomNavigationPage.TitleAlignment.Start);
                CustomNavigationPage.SetTitleFont(this, Font.SystemFontOfSize(NamedSize.Large));
                CustomNavigationPage.SetHasShadow(this, true);
                CustomNavigationPage.SetTitlePadding(this, new Thickness(0, 0, 0, 0));
                //渐变从左到右
                CustomNavigationPage.SetGradientColors(this, new Tuple<Color, Color>(Color.FromHex("#5178bd"), Color.FromHex("#7fadf7")));
                CustomNavigationPage.SetGradientDirection(this, CustomNavigationPage.GradientDirection.RightToLeft);

                this.CurrentPageChanged += Tabs_CurrentPageChanged;


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }

            //MessagingCenter.Unsubscribe<Tradition_Card_Page3ViewModel, string>(this, "page");
            //MessagingCenter.Subscribe<Tradition_Card_Page3ViewModel, string>(this, "page", (arg1, arg2) => {
            //    DisplayAlert("提示",""+ arg2, "Ok");
            //});
            //if (tab != 0)
            //    DisplayAlert("提示", "" + tab, "Ok");

            this.Children.Add(new HomePage()
            {
                Title = "首页",
                Icon = "fa-home"
            });

            this.Children.Add(new SiteMap()
            {
                Title = "位置",
                Icon = "fa-map-marker"
            });

            this.Children.Add(new MyTask()
            {
                Title = "任务",
                Icon = "fa-windows"
            });

            this.Children.Add(new Networks()
            {
                Title = "网点",
                Icon = "fa-bullseye"
            });

            this.Children.Add(new MenuPage()
            {
                Title = "我",
                Icon = "fa-user"
            });

            //int indextab = 0;

            //var pages = Children.GetEnumerator();
            //pages.MoveNext(); // First page
            //pages.MoveNext(); // Second page
            //pages.MoveNext(); // Second page
            //CurrentPage = pages.Current;

            //TabbedPage tp = new TabbedPage();
            //tp.Children.Add(new PageFriends());
            //tp.Children.Add(new PageSnap());
            //tp.Children.Add(new PageNotes());
            //tp.CurrentPage = tp.Children[1];


            //MessagingCenter.Subscribe<NavigationMessage>(this, "tabpage", (args) => {
            //    //DisplayAlert("提示", "" + args.Paremeter, "Ok");
            //    //Tabs_PagesChanged(null,null);
            //    //this.SelectedItem = new Networks()
            //    //{
            //    //    Title = "网点",
            //    //    Icon = "fa-bullseye"
            //    //};
            //    //MessagingCenter.Unsubscribe<NavigationMessage>(this, "tabpage");
            //});

            //
            //if (indextab != 0)
            //    CurrentPage = Children[3];
        }


        public void SetCurrentTab(int index)
        {
            this.CurrentPage = this.Children[index];
        }

        private void Tabs_Appearing(object sender, EventArgs e)
        {
            UpdateNavigationBar(sender);
        }

        private void Tabs_CurrentPageChanged(object sender, System.EventArgs e)
        {
            UpdateNavigationBar(sender);
        }


        private void UpdateNavigationBar(object sender)
        {
            var page = (Xamarin.Forms.Page)sender;

            //System.InvalidOperationException: PopToRootAsync is not supported globally on Android, please use a NavigationPage.


            switch (CurrentPage.Title.Trim())
            {
                case "首页":
                    Title = "普查终端";
                    //Icon = "fa-map-marker";
                    //CustomNavigationPage.SetTitleIcon(this, "icon.png");
                    break;
                case "位置":
                    Title = "定位当前位置";
                    //Icon = "fa-map-marker";
                    //CustomNavigationPage.SetTitleIcon(this, "icon.png");
                    break;
                case "任务":
                    Title = "添加终端普查";
                    Icon = "fa-windows";
                    //CustomNavigationPage.SetTitleIcon(this, "icon.png");
                    break;
                case "网点":
                    Title = "我的网点列表";
                    //Icon = "fa-bullseye";
                    //CustomNavigationPage.SetTitleIcon(this, "icon.png");
                    break;
                case "我":
                    Title = "账户信息";
                    //Icon = "fa-user";
                    //CustomNavigationPage.SetTitleIcon(this, "icon.png");
                    break;
            }
        }



        protected override void OnAppearing()
        {
            SubscribeToMessages();

            //var page = CurrentPage is MyTask;
            //if (App.IndexTab == 3)
            //{
            //    CurrentPage = Children[3];
            //    //App.IndexTab = 0;
            //}

            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            UnsubscribeToMessages();
            base.OnDisappearing();
        }

        //protected async override void OnResume()
        //{
        //    base.OnResume();

        //    await Task.Delay(10);
        //    // Lets start from the beginning again
        //    var nav = ServiceLocator.Current.GetInstance<INavService>();
        //    nav.Home();
        //}

        private void SubscribeToMessages()
        {
            //MessagingCenter.Subscribe<NavigationMessage>(this, "tabpage", (args) => {
            //    DisplayAlert("提示", "" + args.Paremeter, "Ok");
            //    CurrentPage = Children[int.Parse(args.Paremeter.ToString())];
            //});
        }
        private void UnsubscribeToMessages()
        {
            MessagingCenter.Unsubscribe<NavigationMessage>(this, "tabpage");
        }

    }
}