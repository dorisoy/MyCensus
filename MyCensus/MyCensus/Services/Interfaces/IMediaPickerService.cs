using System;
using System.Threading.Tasks;
using System.Threading;

namespace MyCensus.Services.Interfaces
{
    public interface IMediaPickerService
    {
        /// <summary>
        /// 图片转Base64编码
        /// </summary>
        /// <returns></returns>
        Task<string> PickImageAsBase64String();
    }
}
