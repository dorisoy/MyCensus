using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Acr.UserDialogs;
using MyCensus.Controls;
using MyCensus.Models.Census;
using MyCensus.Controls.DialogKit;
using MyCensus.ViewModels.Base;
using MyCensus.Utils;


using MyCensus.PlacesSearchBar;

namespace MyCensus.Views
{
    public partial class AddProducts : ContentPage
    {
        private List<string> products;

        public AddProducts()
        {
            //Title = action + "商品";
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

            var bar = new ToolbarItem("\uf0c7", "", () => { });
            bar.SetBinding(ToolbarItem.CommandProperty, "SaveProductCmd");
            this.ToolbarItems.Add(bar);

            this.AnnualSales.IsEnabled = false;


        }



        private async void InputText_Clicked(object sender, EventArgs e)
        {
            var result = await CrossDiaglogKit.Current.GetInputTextAsync("年销量", "各品牌档次年销量（年/箱）:");
            if (result != null)
            {
                this.AnnualSales.Text = result;
            }
        }


      

        private async Task<IEnumerable<string>> GetSuggestions(string text)
        {
            var result = await Task.Run<IEnumerable<string>>(() =>
            {
                List<string> suggestions = new List<string>();
                return suggestions;
            });
            return result;
        }

      

        //====================================


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

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("提示!", "确定要放弃修改吗?", "是", "否");
                if (result) await this.Navigation.PopAsync();
            });
            return true;
        }

        private void SubscribeToMessages()
        {
            //扫码查询
            MessagingCenter.Subscribe<NavigationMessage>(this, "AutoScanCode", async (args) =>
            {
                ///这里开始查询
                using (UserDialogs.Instance.Loading("查询中..."))
                {
                    var barCodeResult = await BarCodeQueryHelper.AsyncQuery(args.Paremeter.ToString());
                    if (barCodeResult != null)
                    {
                        string product = barCodeResult.showapi_res_body != null ? barCodeResult.showapi_res_body != null ? barCodeResult.showapi_res_body.goodsName : "" : "";
                        string note = barCodeResult.showapi_res_body != null ? barCodeResult.showapi_res_body != null ? barCodeResult.showapi_res_body.note : "" + "/" + barCodeResult.showapi_res_body != null ? barCodeResult.showapi_res_body.spec : "" : "";
                        string manuName= barCodeResult.showapi_res_body != null ? barCodeResult.showapi_res_body != null ? barCodeResult.showapi_res_body.manuName : "" : "";
                        string productNote = (string.IsNullOrEmpty(product) ? "无品名" : product) + " / " + (string.IsNullOrEmpty(note) ? "无规格" : note);
                        //商品名
                        this.ProductName.Text = productNote;

                        //this.ProductName.SelectedItem = new SalesProduct( ProductName= productNote);
                        //topText.SetBinding(Label.TextProperty, new Binding(nameof(TopText)));
                        //this.ProductName.SetBinding(AutoCompleteView.SelectedItemProperty, new Binding(productNote));
                        //this.ProductName.SelectedItem = p;
                        //BarCode
                    }
                    else
                    {
                        await DisplayAlert("提示!", "无登记信息", "确定");
                    }
                }
            });

            MessagingCenter.Subscribe<NavigationMessage>(this, "ConvertAnnualSales", async (args) =>
            {
                //InputText_Clicked(null, null);
                //高档及以上，中档及中档高，中档低、其他
                // "雪花啤酒", "青岛啤酒", "Budweiser百威", "其他"
                string[] options = new string[] { "高档及以上", "中档及中档高", "中档低", "其他" };
                switch (args.Paremeter.ToString())
                {
                    case "雪花":
                        options = new string[] { "高档及以上", "中档及中档高", "中档低", "其他" };
                        break;
                    case "青岛":
                        options = new string[] { "高档及以上", "中档及中档高", "中档低", "其他" };
                        break;
                    case "百威":
                        options = new string[] { "高档及以上", "中档及中档高", "中档低", "其他" };
                        break;
                    case "其他":
                        options = new string[] { "高档及以上", "中档及中档高", "中档低", "其他" };
                        break;
                }
                var result = await CrossDiaglogKit.Current.GetInputTextAsync("年销量", "各品牌档次年销量（年/箱）:", null, GlobalSettings.TempGrade, options);
                if (result != null)
                {
                    this.AnnualSales.IsEnabled = true;
                    this.AnnualSales.Text = string.Format("{0},{1}", this.Brand.SelectedItem, result);
                }
            });

            //搜索订阅
            MessagingCenter.Subscribe<AutoCompletePrediction>(this, "SearchSelectedProduct", (args) =>
            {
                try
                {
                    GlobalSettings.TempGrade = args.Grade;
                    this.ProductName.Text = $"{args.Brand},{args.ProductName}";
                    this.Brand.SelectedIndex = this.Brand.ItemsSource.IndexOf(args.Brand);
                }
                catch (Exception ex)
                { }
            });

        }

        private void UnsubscribeToMessages()
        {
            MessagingCenter.Unsubscribe<NavigationMessage>(this, "ConvertAnnualSales");
            MessagingCenter.Unsubscribe<NavigationMessage>(this, "SearchSelectedProduct");
            MessagingCenter.Unsubscribe<NavigationMessage>(this, "AutoScanCode");
        }


        private async void Handle_OnSuggestionOpen(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            //await MainScroll.ScrollToAsync((Element)sender, ScrollToPosition.End, true);
        }
    }
}
