using System.Collections;
using System.Linq;
using System.Collections.Generic;
using MyCensus.DataServices;
using MyCensus.Models.ReportIncident;
using MyCensus.Views;
//using MyCensus.Views.SignUp;
using MyCensus.ViewModels;
using MyCensus.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyCensus.Controls;
using Prism.Navigation;

namespace MyCensus.Services
{

    /// <summary>
    /// 用于控制全局导航服务扩展方法
    /// </summary>
    public static class NavigationServiceEx 
    {
        static NavigationServiceEx()
        {
            //CreatePageViewModelMappings();
        }

        static readonly Dictionary<Type, Type> _mappings;
        static void CreatePageViewModelMappings()
        {
            try
            {
                //_mappings.Add(typeof(CustomRideViewModel), typeof(CustomRidePage));
                //_mappings.Add(typeof(HomePageViewModel), typeof(HomePage));
                //_mappings.Add(typeof(LoginPageViewModel), typeof(LoginPage));
                //_mappings.Add(typeof(SiteMapViewModel), typeof(SiteMap));
                //_mappings.Add(typeof(MyTaskViewModel), typeof(MyTask));
                //_mappings.Add(typeof(NetworksViewModel), typeof(Networks));
                //_mappings.Add(typeof(AboutPageViewModel), typeof(AboutPage));
                //_mappings.Add(typeof(BookingViewModel), typeof(BookingPage));

                //_mappings.Add(typeof(MyRidesViewModel), typeof(MyRidesPage));
                //_mappings.Add(typeof(ProfileViewModel), typeof(ProfilePage));
                //_mappings.Add(typeof(ReportIncidentViewModel), typeof(ReportIncidentPage));
                //_mappings.Add(typeof(MainPageViewModel), typeof(MainPage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        /// <summary>
        /// 导航到页面
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        public static Task NavigateToAsync<TViewModel>(this INavigationService nav) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(nav,typeof(TViewModel), null);
        }

        /// <summary>
        /// 导航到页面(使用参数)
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Task NavigateToAsync<TViewModel>(this INavigationService nav, object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(nav,typeof(TViewModel), parameter);
        }

        /// <summary>
        /// 导航到页面(视图模型)
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        public static Task NavigateToAsync(this INavigationService nav, Type viewModelType)
        {
            return InternalNavigateToAsync(nav,viewModelType, null);
        }

        /// <summary>
        /// 导航到页面(视图模型,参数)
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Task NavigateToAsync(this INavigationService nav, Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(nav,viewModelType, parameter);
        }

        static async Task InternalNavigateToAsync(this INavigationService nav,  Type viewModelType, object parameter)
        {
            Xamarin.Forms.Page page = CreateAndBindPage(viewModelType, parameter);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }
        static Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }
            return _mappings[viewModelType];

        }
        static Xamarin.Forms.Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            try
            {
                Type pageType = GetPageTypeForViewModel(viewModelType);

                if (pageType == null)
                {
                    throw new Exception($"Mapping type for {viewModelType} is not a page");
                }

                Xamarin.Forms.Page page = Activator.CreateInstance(pageType) as Xamarin.Forms.Page;
                ViewModelBase viewModel = ViewModelLocator.Instance.Resolve(viewModelType) as ViewModelBase;
                page.BindingContext = viewModel;

                if (page is IPageWithParameters)
                {
                    ((IPageWithParameters)page).InitializeWith(parameter);
                }


                return page;
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.Print(ex.Message);
                return null;
            }
        }

        public static Task RemoveLastFromBackStackAsync(this INavigationService nav)
        {
            var mainPage = Application.Current.MainPage;
            if (mainPage != null)
            {
                try
                {
                    var list = new List<Page>();
                    foreach (var page in mainPage.Navigation.NavigationStack)
                    {
                        if (page.GetType().ToString().IndexOf("Tradition_Card_Page1") >= 0 || page.GetType().ToString().IndexOf("Tradition_Card_Page2") >= 0 || page.GetType().ToString().IndexOf("Tradition_Card_Page3") >= 0 || page.GetType().ToString().IndexOf("Restaurant_Card_Page1") >= 0 || page.GetType().ToString().IndexOf("Restaurant_Card_Page2") >= 0 || page.GetType().ToString().IndexOf("Restaurant_Card_Page3") >= 0)
                            list.Add(page);
                    }

                    foreach (var p in list)
                    {
                        mainPage.Navigation.RemovePage(p);
                    }
                    
                }
                catch (Exception ex)
                {
                    throw new Exception($"移除页面错误:{ex.Message}");
                }
            }

            return Task.FromResult(true);
        }


        public static Task RemovePageAsync(this INavigationService nav, string name)
        {
            var mainPage = Application.Current.MainPage;
            if (mainPage != null)
            {
                try
                {
                    foreach (var page in mainPage.Navigation.NavigationStack)
                    {
                        if (page.GetType().ToString().IndexOf(name) >= 0)
                        {
                            mainPage.Navigation.RemovePage(page);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
            }
    
            return Task.FromResult(true);
        }


        public static bool IsCurrentPage(this INavigationService nav, string name)
        {
            bool isExist = false;
            var mainPage = Application.Current.MainPage;
            if (mainPage != null)
            {
                try
                {
                    foreach (var page in mainPage.Navigation.NavigationStack)
                    {
                        if (page.GetType().ToString().IndexOf(name) >= 0)
                        {
                            isExist = true;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isExist = false;
                }
            }
            return isExist;
        }

        public static Page GetPage(this INavigationService nav, string name)
        {
            Page page = null;
            var mainPage = Application.Current.MainPage;
            if (mainPage != null)
            {
                try
                {
                    foreach (var p in mainPage.Navigation.NavigationStack)
                    {
                        if (p.GetType().ToString().IndexOf(name) >= 0)
                        {
                            page = p;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    page = null;
                }
            }
            return page;
        }


    }
}