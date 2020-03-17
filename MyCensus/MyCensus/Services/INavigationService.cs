using System;
using MyCensus.ViewModels;
using System.Threading.Tasks;
using Prism.Navigation;

namespace MyCensus.Services
{
    public interface INavigationService 
    {

        /*
        Task<bool> GoBackAsync();
        Task<bool> GoBackAsync(NavigationParameters parameters);
        Task NavigateAsync(Uri uri);
        Task NavigateAsync(Uri uri, NavigationParameters parameters);
        Task NavigateAsync(string name);
        Task NavigateAsync(string name, NavigationParameters parameters);
        */

        /*
         * 
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
         Task InitializeAsync();

         /// <summary>
         /// 导航到页面
         /// </summary>
         /// <typeparam name="TViewModel"></typeparam>
         /// <returns></returns>
         Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

         /// <summary>
         /// 导航到页面(使用参数)
         /// </summary>
         /// <typeparam name="TViewModel"></typeparam>
         /// <param name="parameter"></param>
         /// <returns></returns>
         Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;

         /// <summary>
         /// 导航到页面(视图模型)
         /// </summary>
         /// <param name="viewModelType"></param>
         /// <returns></returns>
         Task NavigateToAsync(Type viewModelType);

         /// <summary>
         /// 导航到页面(视图模型,参数)
         /// </summary>
         /// <param name="viewModelType"></param>
         /// <param name="parameter"></param>
         /// <returns></returns>
         Task NavigateToAsync(Type viewModelType, object parameter);

         /// <summary>
         /// 回退页面
         /// </summary>
         /// <returns></returns>
         Task NavigateBackAsync();

         /// <summary>
         /// 从后堆栈中移除最后一个页面
         /// </summary>
         /// <returns></returns>
         Task RemoveLastFromBackStackAsync();

     */
    }
}