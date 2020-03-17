using Xamarin.Forms;

namespace MyCensus.Helpers
{
    /// <summary>
    /// 自定义平台类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class OnCustomPlatform<T>
    {
        public OnCustomPlatform()
        {
            Android = default(T);
            iOS = default(T);
            WinPhone = default(T);
            Windows = default(T);
            Other = default(T);
        }

        public T Android { get; set; }

        public T iOS { get; set; }

        public T WinPhone { get; set; }

        public T Windows { get; set; }

        public T Other { get; set; }

        public static implicit operator T(OnCustomPlatform<T> onPlatform)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    return onPlatform.Android;
                case Device.iOS:
                    return onPlatform.iOS;
                case Device.UWP:
                    if(Xamarin.Forms.Device.Idiom == Xamarin.Forms.TargetIdiom.Desktop)
                        return onPlatform.Windows;
                    else
                        return onPlatform.WinPhone;
                default:
                    return onPlatform.Other;
            }
        }
    }
}