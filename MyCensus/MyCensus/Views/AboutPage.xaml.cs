using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyCensus.ViewModels;


namespace MyCensus.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
        //AboutViewModel viewModel;
        public AboutPage ()
		{
			InitializeComponent ();
            //Title = "用户信息";
            
            //var page = this
            NavigationPage.SetBackButtonTitle(this, "我的信息");

            //viewModel = new AboutViewModel();
            //viewModel.Title = "viewModel用户信息";

            //BindingContext = viewModel;
        }
	}
}