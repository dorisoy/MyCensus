using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace MyCensus.Utils
{

    public class CodeResultBody
    {
        public bool flag { get; set; }
        public string remark { get; set; }
        public string code { get; set; }
        public string goodsName { get; set; }
        public string manuName { get; set; }
        public string spec { get; set; }
        public string price { get; set; }
        public string trademark { get; set; }
        public string img { get; set; }
        public string ret_code { get; set; }
        public string goodsType { get; set; }
        public string manuAddress { get; set; }
        public string sptmImg { get; set; }
        public string ycg { get; set; }
        public string engName { get; set; }
        public string note { get; set; }
        public List<string> imgList { get; set; } = new List<string>();
    }

    public class BarCodeResult
    {
        public int? showapi_res_code { get; set; } = 0;
        public string showapi_res_error { get; set; }

        public string showapi_res_id { get; set; }
        public CodeResultBody showapi_res_body { get; set; } = new CodeResultBody();
    }

    public static class BarCodeQueryHelper
    {

        private const String host = "https://ali-barcode.showapi.com";
        private const String path = "/barcode";
        private const String method = "GET";
        private const String appcode = "6706d75dc9b14c03969ca6cafa5504d3";

        public static async Task<BarCodeResult> AsyncQuery(string code)
        {
            return await Task.Run(() => { return Query(code); });
        }

        public static BarCodeResult Query(string code)
        {
            var barCodeResult = new BarCodeResult();

            String querys = "code=" + code + "";
            String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }

            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            try
            {
                Stream responseStream = httpResponse.GetResponseStream();
                using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    var message = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(message))
                    {
                        barCodeResult = JsonConvert.DeserializeObject<BarCodeResult>(message);
                    }
                }
                responseStream.Close();
            }
            catch (Exception)
            { }

            if (httpResponse != null)
                httpResponse.Close();

            if (httpRequest != null)
                httpRequest.Abort();

            return barCodeResult;

        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

    }
}
