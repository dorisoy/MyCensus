using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//using Prism;
//using Prism.Commands;
//using Prism.Navigation;
//using Prism.Ioc;

using MyCensus;
using MyCensus.ViewModels;
using MyCensus.ViewModels.Base;
using MyCensus.Services;


namespace MyCensus.Views
{
    /*
    public partial class LoginPage : ContentPage
    {
        //LoginPageViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "dsafsdf");

            //this.Resources.Add(StyleSheet.FromAssemblyResource(IntrospectionExtensions.GetTypeInfo(typeof(LoginPage)).Assembly,"MyProject.Assets.styles.css"));
            //model
            //viewModel.Title = "LoginPage-xxxx";
            //BindingContext = viewModel;

            //NavigationPage.SetBackButtonTitle(this, "回登入頁面");
            //buttonLogin.Clicked += ButtonLogin_Clicked;
        }
    }
    */

    //[XamlCompilation(XamlCompilationOptions.Skip)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntryUserName.Focus();
        }
    }
}
