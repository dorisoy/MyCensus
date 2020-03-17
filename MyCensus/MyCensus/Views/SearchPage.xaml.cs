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
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
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

            var bar = new ToolbarItem("\uf029", "", () => { });
            bar.SetBinding(ToolbarItem.CommandProperty, "ScanCodeCommand");
            this.ToolbarItems.Add(bar);


            search_bar.PlacesRetrieved += Search_Bar_PlacesRetrieved;
            search_bar.TextChanged += Search_Bar_TextChanged;
            search_bar.MinimumSearchText = 2;
            results_list.ItemSelected += Results_List_ItemSelected;
        }

        void Search_Bar_PlacesRetrieved(object sender, AutoCompleteResult result)
        {
            results_list.ItemsSource = result.AutoCompletePlaces;
            spinner.IsRunning = false;
            spinner.IsVisible = false;
            if (result.AutoCompletePlaces != null && result.AutoCompletePlaces.Count > 0)
                results_list.IsVisible = true;
        }

        void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                results_list.IsVisible = false;
                spinner.IsVisible = true;
                spinner.IsRunning = true;
            }
            else
            {
                results_list.IsVisible = true;
                spinner.IsRunning = false;
                spinner.IsVisible = false;
            }
        }

        async void Results_List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            await Task.Delay(500);

            var prediction = (AutoCompletePrediction)e.SelectedItem;
            results_list.SelectedItem = null;
            //var place = new Place() { Name = "test", Latitude = 0, Longitude = 0 };
            //if (place != null)
            //    await DisplayAlert(place.Name, string.Format("Lat: {0}\nLon: {1}", place.Latitude, place.Longitude), "OK");
            //search_bar.Text = prediction.Description;
            if (prediction != null)
            {
                MessagingCenter.Send<AutoCompletePrediction>(prediction, "SearchSelectedProduct");
                Device.BeginInvokeOnMainThread(async () => {
                    await this.Navigation.PopAsync();
                });
            }
          
            spinner.IsRunning = false;
            spinner.IsVisible = false;
            results_list.IsVisible = false;
        }
    }
}
