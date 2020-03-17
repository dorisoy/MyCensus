using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyCensus.Services.Interfaces
{

    /// <summary>
    /// 自定义工具栏标记
    /// </summary>
    public interface IToolbarItemBadgeService
    {
        void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor);
    }
}
