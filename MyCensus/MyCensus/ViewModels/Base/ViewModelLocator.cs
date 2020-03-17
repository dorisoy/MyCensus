using MyCensus.DataServices;
using MyCensus.DataServices.Base;
using MyCensus.DataServices.Interfaces;
using MyCensus.Services;
using MyCensus.Services.Interfaces;
//using MyCensus.ViewModels.SignUp;
using Prism;
using Prism.Ioc;
using Unity;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;
using System;

namespace MyCensus.ViewModels.Base
{
    public class ViewModelLocator
    {
        //IContainerProvider
        //IContainerRegistry
        private readonly IUnityContainer _unityContainer;
        //private readonly IContainerProvider _unityContainer;


        private static readonly ViewModelLocator _instance = new ViewModelLocator();

        public static ViewModelLocator Instance
        {
            get
            {
                return _instance;
            }
        }

        protected ViewModelLocator()
        {
            _unityContainer = new UnityContainer();
  

            //// Providers
            ////IOperatingSystemVersionProvider
            //_unityContainer.RegisterType<IRequestProvider, RequestProvider>();
            //_unityContainer.RegisterType<ILocationProvider, LocationProvider>();
            //_unityContainer.RegisterType<IMediaPickerService, MediaPickerService>();

            //// Services
            //_unityContainer.RegisterType<IDialogService, DialogService>();
            //RegisterSingleton<INavigationService, NavigationService>();


            //// Data services
            //_unityContainer.RegisterType<IAuthenticationService, AuthenticationService>();
            //_unityContainer.RegisterType<IProfileService, ProfileService>();
            //_unityContainer.RegisterType<IRidesService, RidesService>();
            //_unityContainer.RegisterType<IEventsService, EventsService>();
            //_unityContainer.RegisterType<IWeatherService, OpenWeatherMapService>();
            //_unityContainer.RegisterType<IFeedbackService, FeedbackService>();

            //// View models
            //_unityContainer.RegisterType<CustomRideViewModel>();
            //_unityContainer.RegisterType<CredentialViewModel>();
            //_unityContainer.RegisterType<EventSummaryViewModel>();
            //_unityContainer.RegisterType<GenreViewModel>();
            //_unityContainer.RegisterType<HomeViewModel>();
            //_unityContainer.RegisterType<LoginPageViewModel>();
            //_unityContainer.RegisterType<MainPageViewModel>();
            //_unityContainer.RegisterType<MenuViewModel>();
            //_unityContainer.RegisterType<MyRidesViewModel>();
            //_unityContainer.RegisterType<ProfileViewModel>();
            //_unityContainer.RegisterType<SignUpViewModel>();
            //_unityContainer.RegisterType<UserViewModel>();
            //_unityContainer.RegisterType<ReportIncidentViewModel>();
            //_unityContainer.RegisterType<BookingViewModel>();


            //_unityContainer.RegisterType<SiteMapViewModel>();
            //_unityContainer.RegisterType<MyTaskViewModel>();
            //_unityContainer.RegisterType<NetworksViewModel>();
            //_unityContainer.RegisterType<AboutViewModel>();
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>();
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
