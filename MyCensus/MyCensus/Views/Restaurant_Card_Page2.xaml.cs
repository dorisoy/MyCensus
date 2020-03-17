using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using MyCensus.Controls;

namespace MyCensus.Views
{
    public partial class Restaurant_Card_Page2 : ContentPage
    {
        public Restaurant_Card_Page2()
        {
            Title = "添加餐饮终端普查-经营信息";
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

            ModeOfCooperation.SelectedIndex = ModeOfCooperation.ItemsSource.Count - 1;


            this.PrivateRoomes.Text = "";
            this.TableNumber.Text = "";
            this.Seats.Text = "";
            this.ShowcaseNum.Text = "";

        }

        private void IsAgreement_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
                ModeOfCooperation.SelectedIndex = 0;
            else
                ModeOfCooperation.SelectedIndex = ModeOfCooperation.ItemsSource.Count - 1;
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private async void NextWF_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new Restaurant_Card_Page3(), true);
        //}
    }
}
