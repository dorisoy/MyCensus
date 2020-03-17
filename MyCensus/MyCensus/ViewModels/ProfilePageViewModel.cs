using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using MyCensus.DataServices;
using MyCensus.Models;
using MyCensus.Models.Users;
using MyCensus.Services.Interfaces;
using MyCensus.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


using Prism;



using System.Collections.Generic;
using System.Linq;


using System.ComponentModel;

using MyCensus.DataServices.Interfaces;
using MyCensus.Models.Rides;
using MyCensus.Utils;
using MyCensus.Services;
using MyCensus.Views;
using MyCensus.Cache;

namespace MyCensus.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase, IActiveAware
    {
        private JH_Auth_User _profile;

        private readonly IProfileService _profileService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMediaPickerService _mediaPickerService;
        private readonly ICacheManager _cacheManager;
        public JH_Auth_User Profile
        {
            get
            {
                return _profile;
            }

            set
            {
                _profile = value;
                RaisePropertyChanged(() => Profile);
            }
        }
        
        public ICommand LogoutCommand => new Command(OnLogout);

        public ICommand UpdatePhotoCommand => new Command(OnUpdatePhoto);


        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        public ProfilePageViewModel(IProfileService profileService, 
            IAuthenticationService authenticationService,
            IMediaPickerService mediaPickerService,
            INavigationService navigationService,
            ICacheManager cacheManager,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _profileService = profileService;
            _authenticationService = authenticationService;
            _mediaPickerService = mediaPickerService;
            _cacheManager = cacheManager;

            Title = "个人资料";
        }


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
                await InitAsync();
            }
        }

        public async Task InitAsync()
        {
            IsBusy = true;
            try
            {
                var profile = new JH_Auth_User();
                var cacheProfile = await _cacheManager.Get<JH_Auth_User>(GlobalSettings.profilepage_profile_key);
                if (cacheProfile != null)
                {
                    profile = cacheProfile;
                }
                else
                {
                    profile = await _profileService.GetCurrentProfileAsync();
                    await _cacheManager.Set<JH_Auth_User>(GlobalSettings.profilepage_profile_key, profile);
                }

                if (!IsConnected())
                    _dialogService.LongAlert("网络异常,数据获取失败!");


                if (!string.IsNullOrEmpty(profile.tx))
                {
                    //Force photo reload
                    profile.tx += $"?t={DateTime.Now.Ticks}";
                }
                else
                {
                    profile.tx = "profile_generic.png";
                }

                Profile = profile;
                MessagingCenter.Send(Profile, MessengerKeys.ProfileUpdated);
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching profile with exception: {ex}");
            }

            IsBusy = false;
        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="obj"></param>
        private async void OnLogout(object obj)
        {
            await _authenticationService.LogoutAsync();
            await _navigationService.RemovePageAsync("ProfilePage");
            await _navigationService.RemovePageAsync("MainPage");
            await _navigationService.NavigateAsync("LoginPage?title=LoginPage");
            //await _navigationService.NavigateToAsync<LoginPageViewModel>();
        }


        /// <summary>
        /// 更新照片
        /// </summary>
        /// <param name="obj"></param>
        private async void OnUpdatePhoto(object obj)
        {
            IsBusy = true;

            try
            {
                string imageAsBase64 = await _mediaPickerService.PickImageAsBase64String();
                //上传照片
                await _profileService.UploadUserImageAsync(Profile, imageAsBase64);
                Profile = null;
                await InitAsync();
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error uploading profile image with exception: {ex}");
            }

            IsBusy = false;
        }
    }
}
