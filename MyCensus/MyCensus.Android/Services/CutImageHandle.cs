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


using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.CurrentActivity;

using MyCensus.Droid.Services;
using MyCensus.Services.Interfaces;
using MyCensus.Droid.Activities;


[assembly: Dependency(typeof(CutImageHandle))]
namespace MyCensus.Droid.Services
{
    public class CutImageHandle : ICutImageHandle
    {
        private static Context Context
        {
            get { return Forms.Context ?? Android.App.Application.Context; }
        }

        public void StartCutImage(Action<string> finish, int sourceType)
        {
            Intent _intent = new Intent(Context, typeof(MediaPickerActivity));
            _intent.PutExtra("sourceType", sourceType); //0-相机  1-图库
            Context.StartActivity(_intent);

            MediaPickerActivity.ImageCut += (str) =>
            {
                if (finish != null)
                {
                    finish(str);
                }
            };
        }

    }

}