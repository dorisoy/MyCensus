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

namespace MyCensus.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserPage_old : ContentPage
	{
		public UserPage_old()
		{
			InitializeComponent ();
            Title = "用户信息";

            List<MuenModel> list = new List<MuenModel>();
            list.Add(new MuenModel { Name = "我的二维码", Url = "这里是中文", Image = "mue_Qr_Code.png" });
            list.Add(new MuenModel { Name = "我的钱包", Url = "sss", Image = "mue_Wallet.png" });
            list.Add(new MuenModel { Name = "我的记录", Url = "sss", Image = "mue_Pencil.png" });
            list.Add(new MuenModel { Name = "我的优惠", Url = "sss", Image = "mue_Diploma.png" });
            list.Add(new MuenModel { Name = "设备信息", Url = "sss", Image = "mue_Holding.png" });
            listView.ItemsSource = list;
        }



        private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MuenModel;
            if (item.Name.Equals("我的二维码"))
            {
                await Navigation.PushAsync(new BarcodePage(item.Url));
            }
            else if(item.Name.Equals("设备信息"))
            {
                await Navigation.PushAsync(new Machine());
            }
            else
            {
                var date = await DisplayAlert("提示", item.Name, "同意", "取消");
                await DisplayAlert("提示", "提示内容", date.ToString());
            }
        }
    }
}