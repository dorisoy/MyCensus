using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyCensus.Models;
using MyCensus.ViewModels;
using MyCensus.Helpers;

using Prism.Navigation;

namespace MyCensus.Views
{
    public partial class MyTask : ContentPage
    {
        public MyTask()
        {
            Title = "添加终端普查";
            InitializeComponent();

            //绑定数据
            //List<MuenModel> list = new List<MuenModel>();
            //list.Add(new MuenModel
            //{
            //    Name = "传统终端普查",
            //    Code = "Tradition",
            //    Url = "Tradition_Card",
            //    Image = "",
            //    Icon = "fa-beer"
            //});

            //list.Add(new MuenModel
            //{
            //    Name = "餐饮终端普查",
            //    Code = "Restaurant",
            //    Url = "Restaurant_Card",
            //    Image = "",
            //    Icon = "fa-coffee"
            //});

            //listView.ItemsSource = list;
            //TaskList.BindingContext = list;

            //var tapGestureRecognizer = new TapGestureRecognizer();
            //tapGestureRecognizer.Tapped += (s, e) =>
            //{
            //    System.Diagnostics.Debug.Print(s.ToString());
            //    DisplayAlert("提示", "没有找到页面", "取消");
            //};
        }

        /// <summary>
        /// 终端普查项目选择
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="args"></param>
        //public async void CmdSelectedCommand(Object Sender, TappedEventArgs args)
        //{
        //    //args = {Xamarin.Forms.TappedEventArgs}
        //    string code = args.Parameter.ToString();
        //    //Navigation.PushAsync(new ViewCard(number));
        //    switch (code)
        //    {
        //        case "Tradition":
        //            //传统终端普查
        //            await Navigation.PushAsync(new Tradition_Card_Page1(), true);
        //            break;
        //        case "Restaurant":
        //            //餐饮终端普查
        //            await Navigation.PushAsync(new  Restaurant_Card_Page_One(),true);
        //            break;
        //        case "Kayechang":
        //            //Ka夜场终端普查
        //            //await Navigation.PushAsync(new Kayechang_Card(), true);
        //            await DisplayAlert("提示", "抱歉,功能稍后开放...", "取消");
        //            break;
        //        case "Qudao":
        //            //渠道终端普查
        //            //await Navigation.PushAsync(new Qudao_Card(), true);
        //            await DisplayAlert("提示", "抱歉,功能稍后开放...", "取消");
        //            break;
        //    }
        //}


        ///// <summary>
        ///// 终端普查项目选择
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var item = e.SelectedItem as MuenModel;
        //    if (item.Code.Equals("Tradition"))
        //    {
        //        //传统终端普查
        //        await Navigation.PushAsync(new Tradition_Card_Page1(),true);
        //    }
        //    else if (item.Code.Equals("Restaurant"))
        //    {
        //        //餐饮终端普查
        //        await Navigation.PushAsync(new Restaurant_Card_Page_One(), true);
        //    }
        //    else
        //    {
        //        await DisplayAlert("提示", "没有找到页面", "取消");
        //    }
        //}


    }
}
