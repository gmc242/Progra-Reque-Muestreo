using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAnalytics
{
    public static class AnalyticsConfig
    {
        /// <summary>
        /// The name of your application
        /// </summary>
        public static string AppName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Google.Analytics.AppName"]; }
        }

        /// <summary>
        /// email address to be used for getting analytics data
        /// </summary>
        public static string UserName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Google.Analytics.UserName"]; }
        }

        /// <summary>
        /// legacy password generated from Google for this application to access profile info
        /// </summary>
        public static string AppKey
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Google.Analytics.AppKey"]; }
        }

        /// <summary>
        /// the profileId in format ga:{p# from URL}
        /// </summary>
        public static string ProfileId
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Google.Analytics.ProfileId"]; }
        }

        /// <summary>
        /// the UA analytics code for this web space
        /// </summary>
        public static string AccountId
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["Google.Analytics.AccountId"]; }
        }
    }
}