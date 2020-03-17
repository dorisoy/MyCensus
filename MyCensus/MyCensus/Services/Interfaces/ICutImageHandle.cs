using System;
using System.Threading.Tasks;
using System.Threading;

namespace MyCensus.Services.Interfaces
{
    public interface ICutImageHandle
    {
        /// <summary>
        /// 裁切图片
        /// </summary>
        /// <param name="finish"></param>
        /// <param name="sourceType"></param>
        void StartCutImage(Action<string> finish, int sourceType);
    }
}
