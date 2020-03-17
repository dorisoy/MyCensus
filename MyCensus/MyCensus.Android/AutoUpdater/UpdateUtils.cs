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

using System.IO;
using System.Net;
using System.Threading;
using Android.Content.PM;
using Java.IO;
using Environment = Android.OS.Environment;
using Exception = Java.Lang.Exception;
using File = Java.IO.File;
using Uri = Android.Net.Uri;
using MyCensus.DataServices.Interfaces;

using Android.Support.V4.Content;
using Android.Support.V7.Content;

using Xamarin.Android;

namespace MyCensus.Droid.AutoUpdater
{
    public static class UpdateConfig
    {
        public static Action<Activity, string> AlertFunc = null;

        internal static void Alert(Activity activity, string msg)
        {
            if (AlertFunc != null)
            {
                AlertFunc(activity, msg);
            }
        }
    }

    public class UpdateUtils
    {
        public static string GetVersion(Activity activity)
        {
            try
            {
                PackageManager manager = activity.PackageManager;
                PackageInfo info = manager.GetPackageInfo(activity.PackageName, 0);
                string version = info.VersionName;
                return version;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void Update(Activity activity, UpdateInfo updateInfo)
        {
            try
            {
                string version = GetVersion(activity);
                if (updateInfo.Version == version)
                {
                    UpdateConfig.Alert(activity, activity.GetString(Resource.String.chsword_latest_version));
                }
                else
                {
                    //更新新版本
                    NewVersionUpdate(activity, updateInfo);
                }
            }
            catch (Exception e)
            {
                UpdateConfig.Alert(activity, e.ToString());
            }
        }

        private static void NewVersionUpdate(Activity activity, UpdateInfo updateInfo)
        {
            string version = GetVersion(activity);
            var sb = new StringBuilder();
            sb.AppendFormat(activity.GetString(Resource.String.chsword_current_version), version
                , updateInfo.Version);
            Dialog dialog = new AlertDialog.Builder(activity)
                .SetTitle(activity.GetString(Resource.String.chsword_update_dialog_title))
                .SetMessage(sb.ToString())
                .SetPositiveButton(activity.GetString(Resource.String.chsword_update_dialog_ok),
                    (s, e) =>
                    {
                        ProgressDialog pBar = ProgressDialog.Show(activity, null, "loading", true);
                        pBar.SetTitle(activity.GetString(Resource.String.chsword_update_progress_title));
                        pBar.SetMessage(activity.GetString(Resource.String.chsword_update_progress_content));
                        pBar.SetProgressStyle(ProgressDialogStyle.Spinner);

                        DownloadFile(activity, pBar, updateInfo.DownLoadUrl, pBar);
                    })
                .SetNegativeButton(activity.GetString(Resource.String.chsword_update_dialog_cancel), (s, e) => { }
                ).Create();
            dialog.Show();
        }


        /// <summary>
        /// 下载更新并安装
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="pBar"></param>
        /// <param name="url"></param>
        /// <param name="dialog"></param>
        private static void DownloadFile(Activity activity, ProgressDialog pBar, string url, ProgressDialog dialog)
        {
            pBar.Show();

            var thread = new Thread(() =>
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                using (WebResponse response = webRequest.GetResponse())
                {
                    long totalBytes = 0;

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        //返回主外部存储目录
                        //var file = new File(Environment.ExternalStorageDirectory, filename);

                        string filename = Guid.NewGuid() + ".apk";

                        // 判断文件目录是否存在
                        //外部存储路径
                        //  ==  Download/apk/
                        File tempPath = new File(Application.Context.GetExternalFilesDir(Environment.DirectoryDownloads), "apk");
                        if (!tempPath.Exists())
                        {
                            //创建目录
                            try
                            {
                                tempPath.Mkdir();
                            }
                            catch (Exception e)
                            {
                                e.PrintStackTrace();
                            }
                        }

                        File file = new File(tempPath, filename);//外部存储路径下的apk文件

                        //防止文件过多
                        if (file.Exists())
                        {

                            try
                            {
                                file.Delete();
                            }
                            catch (Exception e)
                            {
                                e.PrintStackTrace();
                            }
                        }

                        if (!file.Exists())
                        {
                            //FileStream fos = new FileStream(filePath, FileMode.Create);
                            using (var dest = new FileOutputStream(file, true))
                            {
                                int currentSize;
                                var buffer = new byte[1024];
                                long totalSize = response.ContentLength;
                                //long totalSize = responseStream.Length;
                                while ((currentSize = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    totalBytes += currentSize;
                                    var percentage = (int)(totalBytes * 100.0 / totalSize);
                                    //dest.Write(buffer, 0, currentBlockSize);
                                    dest.Write(buffer, 0, currentSize);
                                    if (dialog != null)
                                    {

                                        //dialog.Progress = percentage;
                                        activity.RunOnUiThread(() =>
                                        {
                                            dialog.SetMessage(activity.GetString(Resource.String.chsword_update_progress_content) + " " + percentage + "%");
                                        });

                                    }
                                }
                                // installApk(filePath);
                                Down(activity, file, dialog);
                            }
                        }
                    }
                }
            });

            thread.Start();
        }

        /// <summary>
        /// 安装APK文件
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="filename"></param>
        /// <param name="dialog"></param>
        private static void Down(Activity activity, File file, Dialog dialog)
        {
            var context = Application.Context;
            if (!file.Exists())
            {
                return;
            }
            else
            {
                //通过在代码中写入linux指令修改此apk文件的权限，改为全局可读可写可执行
                String[] command = { "chmod", "777", file.Path };
                Java.Lang.ProcessBuilder builder = new Java.Lang.ProcessBuilder(command);
                try
                {
                    builder.Start();
                }
                catch (Java.IO.IOException e)
                {
                    e.PrintStackTrace();
                }
            }

            Uri uri;
            var intent = new Intent(Intent.ActionView);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                uri = FileProvider.GetUriForFile(context, "com.jshcrb.MyCensus.fileprovider", file);
                intent.SetAction(Intent.ActionInstallPackage);
                intent.SetDataAndType(uri, "application/vnd.android.package-archive");
                intent.SetFlags(ActivityFlags.NewTask);
                //FLAG_GRANT_URI_PERMISSION
                intent.AddFlags(ActivityFlags.GrantPersistableUriPermission);
                intent.AddFlags(ActivityFlags.GrantPrefixUriPermission);
                intent.AddFlags(ActivityFlags.GrantWriteUriPermission);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                context.StartActivity(intent);
            }
            else
            {
                uri = Uri.FromFile(file);
                //intent.SetDataAndType(Android.Net.Uri.Parse("file://" + filePath), "application/vnd.android.package-archive");
                intent.SetDataAndType(uri, "application/vnd.android.package-archive");
                intent.SetFlags(ActivityFlags.NewTask);

                context.StartActivity(intent);
            }

            activity.RunOnUiThread(() =>
            {
                dialog.Hide();
                dialog.Dismiss();
                dialog.Cancel();
            });
        }


    }
}