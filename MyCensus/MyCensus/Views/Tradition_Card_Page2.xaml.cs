using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using MyCensus.Controls;
using MyCensus.Controls.ComboBox;

namespace MyCensus.Views
{
    public partial class Tradition_Card_Page2 : ContentPage
    {
        public Tradition_Card_Page2()
        {
            Title = "添加传统终端普查-经营信息";
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

        }


   //

        private void Combo1_SelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            //lbl_combo1.Text = $"Combo1 old index : {e.OldIndex} new index : {e.NewIndex}. Selected Item '{_comboBox1.SelectedItem}'";
        }
        private void Combo2_SelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            //lbl_combo2.Text = $"Combo1 old index : {e.OldIndex} new index : {e.NewIndex}. Selected Item '{_comboBox2.SelectedItem}'";
        }


        private async void IsChain_Focused(object sender, FocusEventArgs e)
        {
            var result = await MyCensus.Controls.DialogKit.CrossDiaglogKit.Current.GetCheckboxResultAsync("Title", "Choose some", "Option 1", "Option 2");
        }

        private void IsAgreement_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
                ModeOfCooperation.SelectedIndex = 0;
            else
                ModeOfCooperation.SelectedIndex = ModeOfCooperation.ItemsSource.Count - 1;

        }
    }
}
