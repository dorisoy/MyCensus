﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using MyCensus.Helpers;
using MyCensus.Models.Events;
using MyCensus.Controls;
using MyCensus.DataServices;
using MyCensus.DataServices.Interfaces;
using MyCensus.Models;
using MyCensus.Models.Rides;
using MyCensus.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using Prism;
using MyCensus.Utils;
using MyCensus.Services;

namespace MyCensus.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        private readonly IEventsService _eventsService;
        private readonly IRidesService _ridesService;
        private new readonly INavigationService _navigationService;
        private new readonly IDialogService _dialogService;

        private bool _showThanks;
     
        public BookingViewModel(IEventsService eventsService, IRidesService ridesService, INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _ridesService = ridesService;
            _eventsService = eventsService;
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

        public bool ShowThanks
        {
            get
            {
                return _showThanks;
            }

            set
            {
                _showThanks = value;
                RaisePropertyChanged(() => ShowThanks);
            }
        }

        private Event _event;

        public Event Event
        {
            get
            {
                return _event;
            }

            set
            {
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }

        private Booking _booking;

        public Booking Booking
        {
            get
            {
                return _booking;
            }

            set
            {
                _booking = value;
                RaisePropertyChanged(() => Booking);
            }
        }

        private TimeSpan _remainingTime;

        public TimeSpan RemainingTime
        {
            get
            {
                return _remainingTime;
            }

            set
            {
                _remainingTime = value;
                RaisePropertyChanged(() => RemainingTime);
            }
        }

        private double _overallProgress;

        public double OverallProgress
        {
            get
            {
                return _overallProgress;
            }

            set
            {
                _overallProgress = value;
                RaisePropertyChanged(() => OverallProgress);
            }
        }





        public override async Task InitializeAsync(object navigationData)
        {
            await LoadData(navigationData);
        }

        private async Task LoadData(object navigationData)
        {
            IsBusy = true;

            try
            {
                var navParameter = navigationData as BookingViewModelNavigationParameter ?? CreateCurrentBookingNavigationParameters();
                ShowThanks = navParameter.ShowThanks;
                Booking = await _ridesService.GetBooking(navParameter.BookingId);

                if (Booking.RideType == RideType.Event && Booking.EventId.HasValue)
                {
                    Event = await _eventsService.GetEventById(Booking.EventId.Value);
                }

                if (ShowThanks)
                {
                    Device.StartTimer(TimeSpan.FromSeconds(5), OnAutomaticReloadTimer);
                }
                else
                {
                    UpdateRemainingTime();
                    Device.StartTimer(TimeSpan.FromSeconds(1), OnRemainigTimer);
                }
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data in: {ex}");
            }

            IsBusy = false;
        }

        private BookingViewModelNavigationParameter CreateCurrentBookingNavigationParameters()
        {
            return new BookingViewModelNavigationParameter()
            {
                ShowThanks = false,
                BookingId = Settings.CurrentBookingId
            };
        }

        private void UpdateRemainingTime()
        {
            var diffTime = Booking.DueDate - DateTime.UtcNow;
            RemainingTime = diffTime > TimeSpan.Zero ? diffTime : TimeSpan.Zero;

            var totalBookingTime = Booking.DueDate - Booking.RegistrationDate;

            OverallProgress = diffTime.TotalSeconds / (totalBookingTime.TotalSeconds * 1.0d);
        }

        private bool OnRemainigTimer()
        {
            if (RemainingTime == TimeSpan.Zero)
            {
                MessagingCenter.Send(Booking, MessengerKeys.BookingFinished);
                _ridesService.RemoveCurrentBooking();
                return false;
            }

            UpdateRemainingTime();
            return true;
        }

        private bool OnAutomaticReloadTimer()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                MessagingCenter.Send(Booking, MessengerKeys.BookingReloadRequest);
                await Task.Delay(500);

                var parameter = new BookingViewModelNavigationParameter
                {
                    ShowThanks = false,
                    BookingId = Booking.Id
                };
                await LoadData(parameter);
            });

            return false;
        }

        public class BookingViewModelNavigationParameter
        {
            public bool ShowThanks { get; set; }

            public int BookingId { get; set; }
        }
    }
}