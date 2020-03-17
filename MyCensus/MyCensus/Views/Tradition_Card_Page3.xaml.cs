using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using System.Diagnostics;
using MyCensus.Controls;
using MyCensus.Models.Census;
using MyCensus.ViewModels.Base;

namespace MyCensus.Views
{
    public partial class Tradition_Card_Page3 : ContentPage
    {
        //public List<TraditionSalesInfo> Products = new List<TraditionSalesInfo>();
        public Tradition_Card_Page3()
        {
            Title = "添加传统终端普查-合作信息";
            InitializeComponent();

            CustomNavigationPage.SetTitleIcon(this, "icon.png");
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

            //


            //var moreAction = new MenuItem { Text = "More" };
            //moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            //moreAction.Clicked += async (sender, e) => {
            //    var mi = ((MenuItem)sender);
            //    Debug.WriteLine("More Context Action clicked: " + mi.CommandParameter);
            //};
            //var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
            //deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            //deleteAction.Clicked += async (sender, e) => {
            //    var mi = ((MenuItem)sender);
            //    Debug.WriteLine("Delete Context Action clicked: " + mi.CommandParameter);
            //};
            //// add to the ViewCell's ContextActions property
            //ContextActions.Add(moreAction);
            //ContextActions.Add(deleteAction);

            //this.ContextDemoList.SeparatorVisibility = SeparatorVisibility.None;
            //this.ContextDemoList.SeparatorColor = Color.Gray;
            //Effects.Add(Effect.Resolve($"MyEffects.ListViewHighlightEffect"));

            this.ContextDemoList.Effects.Add(Effect.Resolve($"MyCensus.ListViewHighlightEffect"));
            //product = new TraditionSalesInfo() { ProductName = "雪花纯生" };
            //Products.Add(product);
            //this.ContextDemoList.ItemsSource = Products;
        }

        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }
        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");

        }

        /// <summary>
        /// 确认提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ConfimWF_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("提示", "提交完成", "确定");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewAdd_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new AddProducts("添加"));
        }

        private void ContextDemoList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //ListView lv = sender as ListView;
            //lv.SelectedItem = null;
        }

        private void ContextDemoList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Course BindingCourse = e.Item as Course;
            //WeekDay BindingWeekDay = (sender as ListView).BindingContext as WeekDay;
            //await Navigation.PushAsync(new DetailPage(MySchedule, BindingWeekDay, BindingCourse));
            //Navigation.PushAsync(new AddProducts("修改"));
        }


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
        
            //MessagingCenter.Subscribe<NavigationMessage>(this, "page", (args) => {
            //    //跳转到新的页面
            //    args.navigationService.NavigateAsync("MainPage");
            //    //DisplayAlert("提示", "" + args.Paremeter, "Ok");
            //    MessagingCenter.Send<NavigationMessage>(new NavigationMessage() { Paremeter = args.Paremeter }, "tabpage");
            //});
        }
        private void UnsubscribeToMessages()
        {
            MessagingCenter.Unsubscribe<NavigationMessage>(this, "page");
        }
    }
}
