using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

using Acr.UserDialogs;
using System.Threading.Tasks;
using Android.Widget;
using Xamarin.Forms;
using MyCensus.Services;


namespace MyCensus.Droid.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }

        public Task<bool> ShowConfirmAsync(string message, string title = null, string okText = null, string cancelText = null)
        {
            //Task<bool> ConfirmAsync(string message, string title = null, string okText = null, string cancelText = null, CancellationToken? cancelToken = null);
            return  UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText);
            //var confirm = await UserDialogs.Instance.ConfirmAsync("确认'开始任务'操作", null, "催促", "返回");
        }
    }
}