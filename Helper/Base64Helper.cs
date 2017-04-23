using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API 
{
    public static class HelperExtensions
    {
        public static bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }
        public static bool OnlyHexInString1(string hex)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            long output;
            return long.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out output);
        }
        public static Boolean IsBase64String(this String value)
        {
                  // Credit: oybek http://stackoverflow.com/users/794764/oybek
            if (value == null || value.Length == 0 || value.Length % 4 != 0
                    || value.Contains(" ") || value.Contains("\t") || value.Contains("\r") || value.Contains("\n"))
                    return false;
            try{
                Convert.FromBase64String(value);
                return true;
            }
            catch(Exception exception){
            // Handle the exception
            }
            return false;
  

            //if (value == null || value.Length == 0 || value.Length % 4 != 0
            //    || value.Contains(' ') || value.Contains('\t') || value.Contains('\r') || value.Contains('\n'))
            //    return false;
            //var index = value.Length - 1;
            //if (value[index] == '=')
            //    index--;
            //if (value[index] == '=')
            //    index--;
            //for (var i = 0; i <= index; i++)
            //    if (IsInvalid(value[i]))
            //        return false;
            //return true;
        }
        // Make it private as there is the name makes no sense for an outside caller
        private static Boolean IsInvalid(char value)
        {
            var intValue = (Int32)value;
            if (intValue >= 48 && intValue <= 57)
                return false;
            if (intValue >= 65 && intValue <= 90)
                return false;
            if (intValue >= 97 && intValue <= 122)
                return false;
            return intValue != 43 && intValue != 47;
        }
    }

}