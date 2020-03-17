using System;
using System.Collections.Generic;
using System.Text;

namespace MyCensus.Controls.AutoComplete
{
    public interface IClueObject
    {
        /// <summary>
        /// 线索字符串表
        /// </summary>
        List<string> ClueStrings { get; }
    }
}
