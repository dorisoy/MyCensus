using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyCensus.Models
{
    public static class Fonts
    {
        public static string IconFont
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        return "IconFont";
                    case Device.Android:
                        return "iconfont.ttf";
                    default:
                        return null;
                }
            }
        }
    }

    public static class IconFonts
    {
        public static readonly string yuyin = "\ue667";
        public static readonly string biaoqing = "\ue611";
        public static readonly string gengduo = "\ue602";
        public static readonly string xiangce = "\ue64e";
        public static readonly string paizhao = "\ue6e5";
        public static readonly string weizhi = "\ue63e";
        public static readonly string fanhui = "\ue607";
        public static readonly string dianhua = "\ue6dd";
        public static readonly string yuyin1 = "\ue605";
        public static readonly string yuyin2 = "\ue69f";
        public static readonly string jianpan = "\ue63f";
        public static readonly string fasong = "\ue60a";
        public static readonly string shanchu = "\ue627";
    }
}
