using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class CreateErrorLogFile
    {

        string sLogFormat;
        string sErrorTime;

        public CreateErrorLogFile()
        {
        }

        public void createerrorlogfiles()
        {

            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message   
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay+".txt";
        }
        public void Errorlog(string sPathname, string sErrormessage)
        {

            StreamWriter sw = new StreamWriter(sPathname + sErrorTime, true);
            sw.WriteLine(sLogFormat + sErrormessage);
            sw.Flush();
            sw.Close();
        }

    }
}