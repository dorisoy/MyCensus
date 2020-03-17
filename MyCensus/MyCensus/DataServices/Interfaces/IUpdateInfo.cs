using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace MyCensus.DataServices.Interfaces
{
    public class UpdateInfo
    {
        public bool Enable { get; set; }
        public string Version { get; set; }
        public string DownLoadUrl { get; set; }
    }



    public interface IUpdateService
    {

        Task<UpdateInfo> GetCurrentVersion();
    }


}
