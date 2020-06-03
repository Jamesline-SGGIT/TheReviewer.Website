using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheReviewer.Website.Services.Common
{
    public static class CommonUtils
    {
        #region DecimalToString
        public static string DeciString(string value)
        {
            return String.Format("{0:F}", Convert.ToDecimal(value));
        }
        #endregion

        #region IsNumeric
        public static bool IsNumeric(string cell)
        {
            decimal myDec;
            var Result = decimal.TryParse(cell, out myDec);
            return Result;
        } 
        #endregion

        #region CastToDate
        public static DateTime? CastToDate(string strDateTime)
        {
            DateTime? dtFinalDateTime = null;
            DateTime dtTempDateTime;
            if (DateTime.TryParse(strDateTime, out dtTempDateTime))
            {
                dtFinalDateTime = dtTempDateTime;
            }

            return dtFinalDateTime;
        }
        #endregion

        #region CastToNullableInteger
        public static int? CastToNullableInteger(string strInteger)
        {
            int? dtFinalInteger = null;
            int intInteger;
            if (Int32.TryParse(strInteger, out intInteger))
            {
                dtFinalInteger = intInteger;
            }

            return dtFinalInteger;
        }
        #endregion

        #region CreateRandomKey
        public static string CreateRandomKey(int KeyLength)
        {
            const string valid = "012389ABCDEFGHIJKLMN4567OPQRSTUVWXYZ";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < KeyLength--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        #endregion

        public static int IntConfromString(string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static Double DoubleConfromString(string value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
       
        public static string FileDater()
        {
            string fileDate;
            fileDate = DateTime.Now.ToString("dd");
            fileDate = fileDate + DateTime.Now.ToString("MM") + "-";
            fileDate = fileDate + DateTime.Now.ToString("HH");
            fileDate = fileDate + DateTime.Now.ToString("mm");
            fileDate = fileDate + DateTime.Now.ToString("ss");
            return fileDate;
        }
    }
}
