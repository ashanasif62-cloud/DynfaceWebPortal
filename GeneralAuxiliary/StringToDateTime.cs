using System;
using System.Data.SqlTypes;

namespace GeneralAuxiliary
{
    public static class StringToDateTime
    {
        private static DateTime ToDateTime(this string _datetime, char _timeSpliter = ':', char _milliSecondSpliter = ',')
        {
            DateTime dateTime = Convert.ToDateTime(SqlDateTime.MinValue.ToString());    //DateTime.Today;
            try
            {
                if (!string.IsNullOrWhiteSpace(_datetime))
                {
                    //char dateSpliter = '-', 
                    _datetime = _datetime.Trim();
                    _datetime = _datetime.Replace("  ", " ");
                    string[] body = _datetime.Split(' ');
                    string[] date = body[0].Split('-');
                    if (date.Length == 1)
                        date = body[0].Split('/');
                    string date_day = date[0];
                    string date_month = date[1];
                    string date_year = date[2];

                    int day = Convert.ToInt32(date_day);
                    int month = 01;
                    if (date_month.Length > 2)
                    {
                        string monthName = date_month.ToLower();
                        switch (monthName)
                        {
                            case ("jan"):
                            case ("january"):
                                month = 01;
                                break;
                            case ("feb"):
                            case ("february"):
                                month = 02;
                                break;

                            case ("mar"):
                            case ("march"):
                                month = 03;
                                break;

                            case ("apr"):
                            case ("april"):
                                month = 04;
                                break;

                            case ("may"):
                                month = 05;
                                break;

                            case ("jun"):
                            case ("june"):
                                month = 06;
                                break;

                            case ("jul"):
                            case ("july"):
                                month = 07;
                                break;

                            case ("aug"):
                            case ("august"):
                                month = 08;
                                break;

                            case ("sep"):
                            case ("september"):
                                month = 09;
                                break;

                            case ("oct"):
                            case ("october"):
                                month = 10;
                                break;

                            case ("nov"):
                            case ("november"):
                                month = 11;
                                break;

                            case ("dec"):
                            case ("december"):
                                month = 12;
                                break;
                        }
                    }
                    else
                    {
                        month = Convert.ToInt32(date_month);
                    }
                    int year = Convert.ToInt32(date_year);
                    int hour = 0, minute = 0, second = 0, millisecond = 0;
                    if (body.Length == 2)
                    {
                        string[] tpart = body[1].Split(_milliSecondSpliter);
                        string[] time = tpart[0].Split(_timeSpliter);
                        hour = Convert.ToInt32(time[0]);
                        minute = Convert.ToInt32(time[1]);
                        if (time.Length == 3) second = Convert.ToInt32(time[2]);
                        if (tpart.Length == 2) millisecond = Convert.ToInt32(tpart[1]);
                    }
                    if (body.Length == 3)
                    {
                        string[] time = body[1].Split(_timeSpliter);
                        hour = Convert.ToInt32(time[0]);
                        minute = Convert.ToInt32(time[1]);

                        string hrsFormat = body[2].ToLower();
                        hour = hrsFormat == "am" ? hour : (hrsFormat == "pm" ? hour : (hour % 12) + 12);

                        if (time.Length == 3) second = Convert.ToInt32(time[2]);
                    }
                    dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
                    return dateTime;
                }
                return dateTime;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);

                if (!string.IsNullOrWhiteSpace(_datetime))
                {
                    DateTime newDateTime = ToDateTime_MMMddYYY(_datetime);
                    return newDateTime;
                }
                return dateTime;
            }
            finally
            { }
        }
        private static DateTime ToDateTime_MMMddYYY(this string _datetime, char _timeSpliter = ':', char _milliSecondSpliter = ',')
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_datetime))
                {
                    //char dateSpliter = '-', 
                    _datetime = _datetime.Trim();
                    _datetime = _datetime.Replace("  ", " ");
                    string[] body = _datetime.Split(' ');
                    string[] date = body[0].Split('-');
                    if (date.Length == 1)
                        date = body[0].Split('/');
                    string date_day = date[1];
                    string date_month = date[0];
                    string date_year = date[2];

                    int day = Convert.ToInt32(date_day);
                    int month = 01;
                    if (date_month.Length > 2)
                    {
                        string monthName = date_month.ToLower();
                        switch (monthName)
                        {
                            case ("jan"):
                            case ("january"):
                                month = 01;
                                break;
                            case ("feb"):
                            case ("february"):
                                month = 02;
                                break;

                            case ("mar"):
                            case ("march"):
                                month = 03;
                                break;

                            case ("apr"):
                            case ("april"):
                                month = 04;
                                break;

                            case ("may"):
                                month = 05;
                                break;

                            case ("jun"):
                            case ("june"):
                                month = 06;
                                break;

                            case ("jul"):
                            case ("july"):
                                month = 07;
                                break;

                            case ("aug"):
                            case ("august"):
                                month = 08;
                                break;

                            case ("sep"):
                            case ("september"):
                                month = 09;
                                break;

                            case ("oct"):
                            case ("october"):
                                month = 10;
                                break;

                            case ("nov"):
                            case ("november"):
                                month = 11;
                                break;

                            case ("dec"):
                            case ("december"):
                                month = 12;
                                break;
                        }
                    }
                    else
                    {
                        month = Convert.ToInt32(date_month);
                    }
                    int year = Convert.ToInt32(date_year);
                    int hour = 0, minute = 0, second = 0, millisecond = 0;
                    if (body.Length == 2)
                    {
                        string[] tpart = body[1].Split(_milliSecondSpliter);
                        string[] time = tpart[0].Split(_timeSpliter);
                        hour = Convert.ToInt32(time[0]);
                        minute = Convert.ToInt32(time[1]);
                        if (time.Length == 3) second = Convert.ToInt32(time[2]);
                        if (tpart.Length == 2) millisecond = Convert.ToInt32(tpart[1]);
                    }
                    if (body.Length == 3)
                    {
                        string[] time = body[1].Split(_timeSpliter);
                        hour = Convert.ToInt32(time[0]);
                        minute = Convert.ToInt32(time[1]);

                        string hrsFormat = body[2].ToLower();
                        hour = hrsFormat == "am" ? hour : (hrsFormat == "pm" ? hour : (hour % 12) + 12);

                        if (time.Length == 3) second = Convert.ToInt32(time[2]);
                    }
                    return new DateTime(year, month, day, hour, minute, second, millisecond);
                }
                return Convert.ToDateTime(SqlDateTime.MinValue.ToString());
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);

                return Convert.ToDateTime(SqlDateTime.MinValue.ToString());
            }
            finally
            { }
        }

        public static DateTime toDateTime(this string _date, bool includeTime = false)
        {
            string date = _date;
            DateTime dateTime = Convert.ToDateTime(SqlDateTime.MinValue.ToString());//DateTime.Today;//
            try
            {
                string[] formats = { "dd/MM/yyyy HH:mm", "dd/MM/yyyy", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "dd/MM/yyyy HH:mm", "dd/MM/yyyy HH:mm:ss tt", "MM/dd/yyyy HH:mm:ss tt", "M/d/yyyy HH:mm:ss tt" };
                if (!string.IsNullOrEmpty(date))
                {
                    //if (includeTime)
                    //{
                    DateTime.TryParseExact(date, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime);
                    //}
                    //else if(dateTime == Convert.ToDateTime(SqlDateTime.MinValue.ToString()))
                    //{
                    //    dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //}
                    //else if (dateTime == Convert.ToDateTime(SqlDateTime.MinValue.ToString()))
                    //{
                    //    dateTime = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    //}
                    //else if (dateTime == Convert.ToDateTime(SqlDateTime.MinValue.ToString()))
                    //{
                    //    dateTime = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    //}
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            return dateTime;
        }


    }
}
