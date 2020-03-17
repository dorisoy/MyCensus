using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyCensus.Models;

using MyCensus.Controls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using MyCensus.ViewModels.Base;
using System.Windows.Input;

namespace MyCensus.Views
{
    public partial class Networks : ContentPage
    {
        //private INavigationService _navigationService;

        public Networks()
        {
            InitializeComponent();

             //var bar = new ToolbarItem("\uf015", "", () =>
            //{
            //    //Device.BeginInvokeOnMainThread(() =>
            //    //{
            //    //    DisplayAlert("提示", "抱歉,功能稍后开放...", "取消");
            //    //});
            //});

            //bar.SetBinding(ToolbarItem.CommandProperty, "GotoHomeCmd");
            //this.ToolbarItems.Remove(bar);
            //MessagingCenter.Subscribe<NavigationMessage>(this, "GotoHomeToolbarItem", (args) =>
            //{
            //    this.ToolbarItems.Add(bar);
            //});
 
            CustomNavigationPage.SetHasNavigationBar(this, true);
            CustomNavigationPage.SetHasBackButton(this, true);

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

            Title = "我的网点列表";
        }

        //public ICommand GotoHomeCmd { protected set; get; }


        /// <summary>
        /// 查看普查详细
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="args"></param>
        //public void OpenView(Object Sender, EventArgs args)
        //{
        //    Button button = (Button)Sender;
        //    string number = button.CommandParameter.ToString();
        //    Navigation.PushAsync(new ViewCard());
        //}
        //ToolbarItems.Remove(ShowCustomRideToolbarItem);

        protected override void OnAppearing()
        {
            SubscribeToMessages();
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            UnsubscribeToMessages();
            base.OnDisappearing();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<NavigationMessage>(this, "GotoHomeToolbarItem", (args) =>
            {
                removePagefromStack("Tradition_Card_Page1");
                removePagefromStack("Tradition_Card_Page2");
                removePagefromStack("Tradition_Card_Page3");
                removePagefromStack("Restaurant_Card_Page1");
                removePagefromStack("Restaurant_Card_Page2");
                removePagefromStack("Restaurant_Card_Page3");
            });
        }

        void removePagefromStack(string pageToRemove)
        {
            try
            {
                foreach (var item in Navigation.NavigationStack)
                {
                    if (item.GetType().Name == pageToRemove)
                    {
                        Navigation.RemovePage(item);
                        break;
                    }
                }
            }
            catch (Exception) { }
        }

        private void UnsubscribeToMessages()
        {
            MessagingCenter.Unsubscribe<NavigationMessage>(this, "GotoHomeToolbarItem");
        }
    }
}
