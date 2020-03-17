using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MyCensus;
using MyCensus.Services;
using MyCensus.DataServices;
using MyCensus.DataServices.Interfaces;
using MyCensus.Droid.AutoUpdater;

namespace MyCensus.Droid.Services
{
    public class OperatingSystemVersionProvider : IOperatingSystemVersionProvider
    {
        private readonly IUpdateService _updateService;

        public OperatingSystemVersionProvider(IUpdateService updateService)
        {
            _updateService = updateService;
        }


        private static Activity mainActivity;

        public string GetOperatingSystemVersionString()
        {
            return Build.VERSION.Release;
        }

        public static void Init(Activity activity)
        {
            mainActivity = activity;
        }

        public async void CheckUpdate()
        {

            //版本更新
            //var updateService = App.ContainerProvider.Resolve<IUpdateService>();
            UpdateInfo updateInfo = await _updateService.GetCurrentVersion();
            UpdateConfig.AlertFunc = new Action<Activity, string>(Complated);
            //
            UpdateUtils.Update(mainActivity, updateInfo);
        }

        private void Complated(Activity mainActivity, string msg)
        {
            //Toast.MakeText(mainActivity, "软件更新完毕", ToastLength.Long).Show();
        }


        public string GetVersion()
        {
            if (mainActivity != null)
                return UpdateUtils.GetVersion(mainActivity);
            else
                return "";
        }



    }
}