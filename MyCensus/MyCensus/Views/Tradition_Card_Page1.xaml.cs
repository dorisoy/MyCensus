using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyCensus.Controls;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;


namespace MyCensus.Views
{
    public partial class Tradition_Card_Page1 : ContentPage
    {
        public Tradition_Card_Page1()
        {
       
            Title = "添加传统终端普查-基本信息";
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
        }

        private async void TirarFoto(object sender, EventArgs e)
        {
            
                await DisplayAlert("Ops", MinhaImagem.ProfileImage.ToString(), "OK");
        }




        //private async void TirarFoto(object sender, EventArgs e)
        //{
        //    await CrossMedia.Current.Initialize();

        //    if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
        //    {
        //        await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

        //        return;
        //    }

        //    var file = await CrossMedia.Current.TakePhotoAsync(
        //        new StoreCameraMediaOptions
        //        {
        //            SaveToAlbum = true,
        //            Directory = "Demo"
        //        });

        //    if (file == null)
        //        return;

        //    MinhaImagem.Source = ImageSource.FromStream(() =>
        //    {
        //        var stream = file.GetStream();
        //        file.Dispose();
        //        return stream;

        //    });
        //}

    }
}
