using System;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Content;
using Android.Views;
using Android.Widget;

using Plugin.CurrentActivity;

using MyCensus;
using HockeyApp.Android;

namespace MyCensus.Droid
{
    //可以在该属性中指定附加应用程序信息

//#if DEBUG
//    [Application(Debuggable = true)]
//#else
//	    [Application(Debuggable = false)]
//#endif

    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        //handle=javaReference
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {

        }



        public override void OnCreate()
        {

    
            base.OnCreate();
            //注册HockeyApp 用于收集系统崩溃信息
            RegisterActivityLifecycleCallbacks(this);
            //HockeyApp.Android.CrashManager.Register(this, GlobalSettings.HockeyAppAPIKeyForAndroid, new AutoReportingCrashManagerListener());
            //注册未处理异常事件
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            //CrashHandler crashHandler = CrashHandler.GetInstance();
            //crashHandler.Init(ApplicationContext);
        }

        void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            UnhandledExceptionHandler(e.Exception, e);
        }

    
        /// <summary>
        /// 处理未处理异常
        /// </summary>
        /// <param name="e"></param>
        private void UnhandledExceptionHandler(Exception ex, RaiseThrowableEventArgs e)
        {
            //处理程序（记录 异常、设备信息、时间等重要信息）
            //**************
            //提示
            Task.Run(() =>
            {
                Looper.Prepare();
                //可以换成更友好的提示
                //Toast.MakeText(this, "很抱歉,程序出现异常,即将退出." + ex.Message + "; " + ex.StackTrace, ToastLength.Long).Show();
                Toast.MakeText(this, "很抱歉,程序出现异常,即将退出.", ToastLength.Long).Show();
                Looper.Loop();
            });

            //停一会，让前面的操作做完
            System.Threading.Thread.Sleep(2000);

            e.Handled = true;
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        protected override void Dispose(bool disposing)
        {
            AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironment_UnhandledExceptionRaiser;
            base.Dispose(disposing);
        }
    }

	internal class AutoReportingCrashManagerListener : CrashManagerListener 
	{
        /// <summary>
        /// 自动上传崩溃信息
        /// </summary>
        /// <returns></returns>
		public override bool ShouldAutoUploadCrashes() => true;
	}
}