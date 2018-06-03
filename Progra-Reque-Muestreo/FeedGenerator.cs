using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using GoogleAnalytics.Json;
using Google.GData.Analytics;
using Google.GData.Client;
using Google.GData.Extensions;


namespace GoogleAnalytics
{
    public class FeedGenerator
    {
        private string _baseUrl = "https://www.google.com/analytics/feeds/data";
        private DataFeed queryResults;

        public AnalyticsService AnalyticsService;
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string GaProfileId { set; get; }

        /// <summary>
        /// Initializes a new instance of the FeedGenerator
        /// </summary>
        public FeedGenerator()
        {
            AnalyticsService = new AnalyticsService(AnalyticsConfig.AppName);
            AnalyticsService.setUserCredentials(AnalyticsConfig.UserName, AnalyticsConfig.AppKey);
            GaProfileId = AnalyticsConfig.ProfileId;
        }

        /// <summary>
        /// Sets the start and end dates for all queries to run against
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void SetGenericQueryProperties(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        private string DateAsGAString(DateTime value)
        {
            return value.Year + "-" + value.Month.ToString("00") + "-" + value.Day.ToString("00");
        }

        /// <summary>
        /// Runs a Query against the Google Analytics provider - AnalyticsService
        /// </summary>
        /// <param name="dimensions">multiple dimensions separated by a comma: ga:dimensionName, ga:dimensionName</param>
        /// <param name="metrics">multiple metrics separated by a comma: ga:metricName, ga:metricName</param>
        /// <param name="numberToRetrieve">the max number of entries to return</param>
        public void RunQuery(string dimensions, string metrics, int numberToRetrieve = default(int))
        {
            try
            {
                // GA Data Feed query uri.
                DataQuery query = new DataQuery(_baseUrl);
                query.Ids = GaProfileId;
                query.Dimensions = dimensions; //"ga:date";
                query.Metrics = metrics; //"ga:visitors, ga:newVisits, ga:bounces";
                //query.Segment = "gaid::-11";
                //query.Filters = "ga:medium==referral";
                //query.Sort = "-ga:visits";
                //query.NumberToRetrieve = 5;
                if (numberToRetrieve != default(int))
                {
                    query.NumberToRetrieve = numberToRetrieve;
                }
                query.GAStartDate = DateAsGAString(StartDate);
                query.GAEndDate = DateAsGAString(EndDate);
                Uri url = query.Uri;

                // Send our request to the Analytics API and wait for the results to
                // come back.
                queryResults = AnalyticsService.Query(query);
            }
            catch (AuthenticationException ex)
            {
                throw new Exception("Authentication failed : " + ex.Message);
            }
            catch (Google.GData.Client.GDataRequestException ex)
            {
                throw new Exception("Authentication failed : " + ex.Message);
            }
        }

        /// <summary>
        /// Converts Google.Analytics.DataFeed into a JsonDataTable
        /// </summary>
        /// <param name="jsKey">a unique key for the javascript variable.  variable is created as var {jsKey}_JsonTable = {};</param>
        /// <returns></returns>
        public string ResultsAsJsonTable(string jsKey)
        {
            string x = queryResults.ToString();

            if (queryResults.Entries.Count == 0)
            {
                return "{}";
            }

            JsonTable tab = new JsonTable(jsKey);
            int idx = 0;
            foreach (DataEntry entry in queryResults.Entries)
            {
                JsonRow row = new JsonRow();
                foreach (Dimension dimension in entry.Dimensions)
                {
                    if (idx == 0)
                    {
                        tab.Columns.Add(new JsonColumn() { ID = dimension.Name, Label = getLabelFriendlyName(dimension.Name), Type = JsonType.STRING });
                    }
                    JsonCell cell = new JsonCell();
                    cell.Type = JsonType.STRING;
                    int valAsNumDate;
                    if (int.TryParse(dimension.Value, out valAsNumDate))
                    {
                        string year = dimension.Value.Substring(0, 4);
                        string month = dimension.Value.Substring(4, 2);
                        string day = dimension.Value.Substring(6);
                        cell.Value = month + '/' + day + '/' + year;
                    }
                    else
                    {
                        cell.Value = dimension.Value;
                    }
                    row.Cells.Add(cell);
                }
                foreach (Metric metric in entry.Metrics)
                {
                    if (idx == 0)
                    {
                        tab.Columns.Add(new JsonColumn() { ID = metric.Name, Label = getLabelFriendlyName(metric.Name), Type = JsonType.NUMBER });
                    }
                    JsonCell cell = new JsonCell();
                    cell.Type = JsonType.NUMBER;
                    cell.Value = metric.Value;
                    row.Cells.Add(cell);
                }

                idx++;
                tab.Rows.Add(row);
            }

            return tab.ToString();
        }

        private string getLabelFriendlyName(string value)
        {
            string retVal = value.Substring(3);

            if (string.IsNullOrWhiteSpace(retVal))
                return string.Empty;
            StringBuilder newText = new StringBuilder(retVal.Length * 2);
            newText.Append(retVal[0]);
            for (int i = 1; i < retVal.Length; i++)
            {
                if (char.IsUpper(retVal[i]) && retVal[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(retVal[i]);
            }
            return newText.ToString().ToLower();
        }
    }
}