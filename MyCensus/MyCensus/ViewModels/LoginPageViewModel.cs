using Prism;
using Prism.Commands;
using Prism.Navigation;

using MyCensus.DataServices;
using MyCensus.DataServices.Base;
using MyCensus.Validations;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MyCensus.Services;
using Xamarin.Forms;
using MyCensus.Controls;
using MyCensus.Views;

namespace MyCensus.ViewModels
{

    public class LoginPageViewModel : ViewModelBase, IActiveAware
    {
        private ValidatableObject<string> _userName;
        private ValidatableObject<string> _password;
        private bool _isValid;
        private bool _isEnabled;
        private readonly IAuthenticationService _authenticationService;
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        public LoginPageViewModel(INavigationService navigationService, IDialogService dialogService, IAuthenticationService authenticationService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _authenticationService = authenticationService;

            _userName = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            AddValidations();
        }


        public ValidatableObject<string> UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }


        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        private bool Validate()
        {
            bool isValidUser = _userName.Validate();
            bool isValidPassword = _password.Validate();
            return isValidUser && isValidPassword;
        }

        private void Enable()
        {
            IsEnabled = !string.IsNullOrEmpty(UserName.Value) &&
                !string.IsNullOrEmpty(Password.Value);
        }

        private void AddValidations()
        {
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Username should not be empty" });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password should not be empty" });
        }



        /// <summary>
        /// 登陆验证
        /// </summary>
        private DelegateCommand<EventArgs> _validateCommand;
        public DelegateCommand<EventArgs> ValidateCommand
        {
            get
            {
                if (_validateCommand == null)
                {
                    //Enable
                    _validateCommand = new DelegateCommand<EventArgs>(r =>
                    {
                        Enable();
                    });
                }

                return _validateCommand;
            }
        }

        /// <summary>
        /// 登录操作绑定的命令
        /// </summary>
        private DelegateCommand<EventArgs> _loginCommand;
        public DelegateCommand<EventArgs> LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    //_loginCommand = new DelegateCommand<EventArgs>(async r =>
                    //{

                    //    //这里做登录操作，如访问你的WebApi
                    //    await Task.Delay(2000);
                    //    //这里假如已经做完登录操作，保存用户信息，并跳转到MainPage；
                    //    await _navigationService.NavigateAsync("CustomNavigationPage/MainPage");
                    //});
                    _loginCommand = new DelegateCommand<EventArgs>(r => { SignInAsync(); });
                }
                return _loginCommand;

            }
        }


        private async void SignInAsync()
        {

            IsBusy = true;
            IsValid = true;
            bool isValid = Validate();
            bool isAuthenticated = false;


            using (UserDialogs.Instance.Loading("登陆中..."))
            {
                await Task.Delay(1000);
                if (isValid)
                {
                    try
                    {
                        isAuthenticated = await _authenticationService.LoginAsync(UserName.Value, Password.Value);
                    }
                    catch (ServiceAuthenticationException)
                    {
                        await _dialogService.ShowAlertAsync("Invalid credentials", "Login failure", "Try again");
                    }
                    catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
                    {
                        Debug.WriteLine($"[SignIn] Error signing in: {ex}");
                        //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[SignIn] Error signing in: {ex}");
                        await _dialogService.ShowAlertAsync("系统错误", ex.Message, "确定");
                    }
                }
                else
                {
                    IsValid = false;
                }
                await Task.Delay(1000);
            }

            if (isAuthenticated)
            {
                //var pages = Application.Current.MainPage.Navigation.NavigationStack;
                if (_navigationService.IsCurrentPage("MainPage"))
                {
                    //var mainPage = _navigationService.GetPage("LoginPage");
                    //await mainPage.Navigation.PopAsync();
                    await Application.Current.MainPage.Navigation.PopAsync();
                    //MainPage : BottomTabbedPage
                }
                else
                {
                    await _navigationService.NavigateAsync("CustomNavigationPage/MainPage");
                }
                //await _navigationService.NavigateToAsync<MainPageViewModel>();
                //NavigationService.NavigateAsync("LoginPage?title=LoginPage");
                //await _navigationService.GoBackToRootAsync();
            }

            IsBusy = false;
        }

        /*

       
        

        public ICommand SignInCommand => new Command(SignInAsync);

        public ICommand GoToSignUpCommand => new Command(GoToSignUp);

        public ICommand ValidateCommand
        {
            get { return new Command(() => Enable()); }
        }

        

        private async void GoToSignUp()
        {
            await NavigationService.NavigateToAsync<SignUpViewModel>();
        }

       

        public override Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

    */

        public event EventHandler IsActiveChanged;
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnActiveTabChangedAsync();
            }
        }
        private async void OnActiveTabChangedAsync()
        {
            if (IsActive)
            {
                //var series = await _tsApiService.GetStatsTopSeries();
                await Task.Delay(100);
            }
        }

    }
}
