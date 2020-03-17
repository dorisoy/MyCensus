using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using MyCensus.DataServices.Interfaces;
using MyCensus.Models.ReportIncident;
using MyCensus.Validations;
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
using MyCensus.Models.Rides;
using MyCensus.Utils;
using MyCensus.Services;


namespace MyCensus.ViewModels
{
    public class ReportIncidentPageViewModel : ViewModelBase, IActiveAware
    {
        private bool _handlebar;
        private bool _fork;
        private bool _pedals;
        private bool _flatTire;
        private bool _chain;
        private bool _loss;
        private ReportedIssueType _reportIncidentType;
        private ValidatableObject<string> _title;
        private ValidatableObject<string> _description;
        private bool _isValid;
        private IFeedbackService _feedbackService;

        public bool Handlebar
        {
            get
            {
                return _handlebar;
            }
            set
            {
                _handlebar = value;
                RaisePropertyChanged(() => Handlebar);
            }
        }

        public bool Fork
        {
            get
            {
                return _fork;
            }
            set
            {
                _fork = value;
                RaisePropertyChanged(() => Fork);
            }
        }

        public bool Pedals
        {
            get
            {
                return _pedals;
            }
            set
            {
                _pedals = value;
                RaisePropertyChanged(() => Pedals);
            }
        }

        public bool FlatTire
        {
            get
            {
                return _flatTire;
            }
            set
            {
                _flatTire = value;
                RaisePropertyChanged(() => FlatTire);
            }
        }

        public bool Chain
        {
            get
            {
                return _chain;
            }
            set
            {
                _chain = value;
                RaisePropertyChanged(() => Chain);
            }
        }

        public bool Loss
        {
            get
            {
                return _loss;
            }
            set
            {
                _loss = value;
                RaisePropertyChanged(() => Loss);
            }
        }

        public new ValidatableObject<string> Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public ValidatableObject<string> Description
        {
            get
            {
                return _description;
            }       
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
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

        public ICommand HandleBarCommand
        {
            get { return new Command<string>((r) => SetReportType(ReportedIssueType.Handlebar)); }
        }

        public ICommand ForkCommand
        {
            get { return new Command<string>((r) => SetReportType(ReportedIssueType.Fork)); }
        }

        public ICommand PedalsCommand
        {
            get { return new Command<string>((r) => SetReportType(ReportedIssueType.Pedals)); }
        }

        public ICommand FlatTireCommand
        {
            get { return new Command<string>((r) => SetReportType(ReportedIssueType.FlatTire)); }
        }

        public ICommand ChainCommand
        {
            get { return new Command<string>((r) => SetReportType(ReportedIssueType.Chain)); }
        }

        public ICommand LossCommand
        {
            get { return new Command<string>((r) => SetReportType(ReportedIssueType.Stolen)); }
        }

        public ICommand ValidateCommand
        {
            get { return new Command(() => CheckValidation()); }
        }

        public ICommand ReportCommand
        {
            get { return new Command(() => Report()); }
        }

        public ICommand OpenBotCommand
        {
            get { return new Command(() => OpenBot()); }
        }



        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        public ReportIncidentPageViewModel(IFeedbackService feedbackService,INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _feedbackService = feedbackService;

            _title = new ValidatableObject<string>();
            _description = new ValidatableObject<string>();

            AddValidations();
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
                //var series = await _tsApiService.GetStatsTopSeries();
                await Task.Delay(100);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsValid = false;

            return base.InitializeAsync(navigationData);
        }

        public bool Validate()
        {
            bool isValidTitle = _title.Validate();
            bool isValidDescription = _description.Validate();
            bool isIncidentValid = _handlebar || _fork || _pedals || _flatTire || _chain || _loss;

            return isValidTitle && isValidDescription && isIncidentValid;
        }

        private void AddValidations()
        {
            _title.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Title should not be empty" });
            _description.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Description should not be empty" });
        }

        private void CheckValidation()
        {
            IsValid = Validate();
        }

        private void SetReportType(ReportedIssueType type)
        {
            Chain = false;
            FlatTire = false;
            Fork = false;
            Handlebar = false;
            Loss = false;
            Pedals = false;

            switch (type)
            {
                case ReportedIssueType.Chain:
                    Chain = true;
                    break;
                case ReportedIssueType.FlatTire:
                    FlatTire = true;
                    break;
                case ReportedIssueType.Fork:
                    Fork = true;
                    break;
                case ReportedIssueType.Handlebar:
                    Handlebar = true;
                    break;
                case ReportedIssueType.Stolen:
                    Loss = true;
                    break;
                case ReportedIssueType.Pedals:
                    Pedals = true;
                    break;
            }

            _reportIncidentType = type;
        }

        private async void Report()
        {
            IsBusy = true;

            try
            {
                IsValid = true;

                bool isValid = Validate();

                if (isValid)
                {
                    var incident = new ReportedIssue
                    {
                        Type = _reportIncidentType,
                        Title = Title.Value,
                        Description = Description.Value
                    };

                    await _feedbackService.SendIssueAsync(incident);
                    MessagingCenter.Send(incident, MessengerKeys.ReportSent);
                    ResetData();
                }
                else
                {
                    IsValid = false;
                }
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reporting incident in: {ex}");
            }

            IsBusy = false;
        }

        private void ResetData()
        {
            SetReportType(ReportedIssueType.Unknown);
            Title.Value = Description.Value = null;
        }

        private async void OpenBot()
        {
            try
            {
                Device.OpenUri(new Uri(GlobalSettings.SkypeBotAccount));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in: {ex}");
                await _dialogService.ShowAlertAsync("Unable to launch Skype.", "Error", "Ok");
            }
        }
    }
}