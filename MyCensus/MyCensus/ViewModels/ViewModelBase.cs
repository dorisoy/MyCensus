

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyCensus.Services;
using MyCensus.ViewModels.Base;
using System.Threading.Tasks;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using System.Linq.Expressions;
using System.Reflection;

using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
//CrossConnectivity
namespace MyCensus.ViewModels
{

    public abstract class ExtendedBindableObject : BindableObject//BindableBase//
    {
        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = GetMemberInfo(property).Name;
            OnPropertyChanged(name);
        }

        private MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            if (lambdaExpression.Body as UnaryExpression != null)
            {
                UnaryExpression body = (UnaryExpression)lambdaExpression.Body;
                operand = (MemberExpression)body.Operand;
            }
            else
            {
                operand = (MemberExpression)lambdaExpression.Body;
            }
            return operand.Member;
        }
    }

    public class ViewModelBase : ExtendedBindableObject, INavigationAware, IDestructible
    {
        protected INavigationService _navigationService;
        protected IDialogService _dialogService;


        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        //public NavigationParameters Parameters { get; set; }


        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public bool IsConnected()
        {

            return CrossConnectivity.Current.IsConnected;

        }


        public ViewModelBase(INavigationService navigationService,IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            CrossConnectivity.Current.ConnectivityTypeChanged += OnConnectivityTypeChanged;
        }

        private void OnConnectivityTypeChanged(object sender, ConnectivityTypeChangedEventArgs connectivityTypeChangedEventArgs)
        {
            try
            {
                var connectionDetails = "网络连接失败,请重新尝试!";

                if (connectivityTypeChangedEventArgs.IsConnected)
                    connectionDetails = string.Format("网络{0}已经连接!", CrossConnectivity.Current.ConnectionTypes.FirstOrDefault());

                _dialogService.ShortAlert(connectionDetails);
            }
            catch (Exception ex)
            {
                _dialogService.LongAlert(ex.Message);
            }
        }


        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            //Parameters = parameters;
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            //Parameters = parameters;
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            //if (parameters.ContainsKey("title"))
            //    Title = (string)parameters["title"] + " and Prism";
            //Parameters = parameters;
        }

        public virtual void Destroy()
        {

        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }

}
