using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;

namespace CheatGameApp
{
    public static class Extensions
    {
        public static void FromString(this IPEndPoint ipEndPoint, string s)
        {
            string[] values = s.Split(':');
            ipEndPoint.Address = IPAddress.Parse(values[0]);
            ipEndPoint.Port = int.Parse(values[1]);
        }
        /// <summary>
        /// Returns a formated string value with the "N2$" format
        /// </summary>
        public static string ToCash(this double cash)
        {
            return "$" + cash.ToString("N2");
        }
        public static string ToStringX(this TimeSpan timeSpan, string format)
        {
            string s = format.ToLower();
            s = s.Replace("dd", timeSpan.Days.ToString("00"));
            s = s.Replace("hh", timeSpan.Hours.ToString("00"));
            s = s.Replace("mm", timeSpan.Minutes.ToString("00"));
            s = s.Replace("ss", timeSpan.Seconds.ToString("00"));
            s = s.Replace("fff", timeSpan.Milliseconds.ToString("000"));
            return s;
        }
        public static string ToTimeString(this TimeSpan timeSpan)
        {
            return String.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }
        public static bool GetParamBoolean(this XmlDocument doc, string nodeName)
        {
            return bool.Parse(GetParamString(doc, nodeName));
        }
        public static string GetParamString(this XmlDocument doc, string nodeName)
        {
            string value = doc.DocumentElement.GetElementsByTagName(nodeName)[0].Attributes["value"].Value;
            return value;
        }
        public static int GetParamInt32(this XmlDocument doc, string nodeName)
        {
            return int.Parse(GetParamString(doc, nodeName));
        }
        public static double GetParamDouble(this XmlDocument doc, string nodeName)
        {
            return double.Parse(GetParamString(doc, nodeName));
        }
        public static TimeSpan GetParamTimeSpan(this XmlDocument doc, string nodeName)
        {
            return TimeSpan.FromSeconds(int.Parse(GetParamString(doc, nodeName)));
        }
    }
}
