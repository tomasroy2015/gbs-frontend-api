using Business;
using GBSHotels.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace GBSHotels.API.Controllers
{
    public class BaseApiController : ApiController
    {
        public HttpResponseMessage LogError(Exception ex)
        {
            string hostName = Dns.GetHostName();
            string GetUserIPAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();

            //string GetUserIPAddress = GetUserIPAddress1();
            using (BaseRepository baseRepo = new BaseRepository())
            {
                //BizContext BizContext1 = new BizContext();
                BizApplication.AddError(baseRepo.BizDB, "Home", ex.Message, ex.StackTrace, DateTime.Now, GetUserIPAddress);
            }

            CreateErrorLogFile CreateErrorLogFile = new CreateErrorLogFile();
            CreateErrorLogFile.createerrorlogfiles();
            CreateErrorLogFile.Errorlog(System.Web.HttpContext.Current.Server.MapPath("\\LogFiles\\Logs"), ex.Source.Length.ToString() + ex.StackTrace + ex.Message);

            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        }

        public string ConvertStringToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public string ConvertHexToString(string HexValue)
        {
            string original = HexValue;
            try
            {
                string StrValue = "";
                while (HexValue.Length > 0)
                {
                    StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
                    HexValue = HexValue.Substring(2, HexValue.Length - 2);
                }
                return StrValue;
            }
            catch(Exception ex)
            {
                return original;
            }
        }

    
            public static string ToHexString(string str)
            {
                var sb = new StringBuilder();

                var bytes = Encoding.Unicode.GetBytes(str);
                foreach (var t in bytes)
                {
                    sb.Append(t.ToString("X2"));
                }

                return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
            }

            public static string FromHexString(string hexString)
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
            }
        
    }
}
