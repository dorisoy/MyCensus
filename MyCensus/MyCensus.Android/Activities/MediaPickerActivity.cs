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

namespace MyCensus.Droid.Activities
{
    [Activity]
    public class MediaPickerActivity : Activity
    {
        public static Action<string> ImageCut;

        Java.IO.File sdcardTempFile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            sdcardTempFile = new Java.IO.File("/mnt/sdcard/", "tmp_pic_" + SystemClock.CurrentThreadTimeMillis() + ".jpg");
            //0-相机  1-图库
            //if (Intent.GetIntExtra("sourceType", 1) == 0)
            //{
            //    CutImageByCamera();
            //}
            //else if (Intent.GetIntExtra("sourceType", 1) == 1)
            //{
            //    CutImageByImgStore();
            //}
            CutImageByCamera();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == 3) //剪裁
                {
                    if (ImageCut != null)
                    {
                        ImageCut(sdcardTempFile.AbsolutePath);
                    }
                }
            }

            Finish();
        }


        //private void CutImageByImgStore()
        //{
        //    Intent _intentCut = new Intent("android.intent.action.PICK");
        //    _intentCut.SetDataAndType(MediaStore.Images.Media.InternalContentUri, "image/*");
        //    _intentCut.PutExtra("output", Android.Net.Uri.FromFile(sdcardTempFile));
        //    _intentCut.PutExtra("crop", "true");
        //    _intentCut.PutExtra("aspectX", 1);// 裁剪框比例
        //    _intentCut.PutExtra("aspectY", 1);
        //    _intentCut.PutExtra("outputX", 180);// 输出图片大小
        //    _intentCut.PutExtra("outputY", 180);
        //    this.StartActivityForResult(_intentCut, 3);
        //}

        private void CutImageByCamera()
        {
            Intent intent = new Intent("android.media.action.IMAGE_CAPTURE");
            intent.PutExtra("output", Android.Net.Uri.FromFile(sdcardTempFile));
            intent.PutExtra("crop", "true");
            intent.PutExtra("aspectX", 1);// 裁剪框比例
            intent.PutExtra("aspectY", 1);
            intent.PutExtra("outputX", 180);// 输出图片大小
            intent.PutExtra("outputY", 180);
            StartActivityForResult(intent, 3);
        }

    }

}