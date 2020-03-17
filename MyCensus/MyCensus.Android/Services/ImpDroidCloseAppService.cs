using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MyCensus.Services;

namespace MyCensus.Droid.Services
{
    public class ImpDroidCloseAppService: ICloseAppService
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }

    //public class ImpiOSCloseAppService : ICloseAppService
    //{
    //    public void CloseApp()
    //    {
    //        Thread.CurrentThread.Abort();
    //    }
    //}

    //public class ImpUWPCloseAppService : ICloseAppService
    //{
    //    public void CloseApp()
    //    {
    //        Application.Current.Exit();
    //    }
    //}
}
