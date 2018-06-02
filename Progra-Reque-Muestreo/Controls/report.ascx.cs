using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoogleAnalytics.Json;

namespace GoogleAnalytics.Controls
{
    public partial class report : System.Web.UI.UserControl
    {
        private FeedGenerator _generator = new FeedGenerator();

        /// <summary>
        /// Unique Name of this Query
        /// </summary>
        public string QueryName { set; get; }
        /// <summary>
        /// The Start Date for this report
        /// </summary>
        public DateTime StartDate { set; get; }
        /// <summary>
        /// The End Date for this report
        /// </summary>
        public DateTime EndDate { set; get; }
        /// <summary>
        /// A comma separated list of dimensions in the format ga:[A-z], ...
        /// <example>ga:source, ga:keyword, ga:referralPath, ga:socialNetwork</example>
        /// </summary>
        public string Dimensions { set; get; }
        /// <summary>
        /// A comma separated list of metrics in the format ga:[A-z], ...
        /// <example>ga:entrances, ga:pageViews</example>
        /// </summary>
        public string Metrics { set; get; }
        /// <summary>
        /// The Type of Chart which this query should be displayed as
        /// </summary>
        public ChartType ChartType { set; get; }
        /// <summary>
        /// Any pre-defined Google Chart Options
        /// <see cref="https://google-developers.appspot.com/chart/interactive/docs/basic_customizing_chart"/>
        /// </summary>
        public ChartOptions ChartOptions { set; get; }
        /// <summary>
        /// Size for Paged Datasets
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// Formatted JSON data to be displayed in the chart
        /// </summary>
        public string Json { private set; get; }

        public override void DataBind()
        {
            if (string.IsNullOrEmpty(QueryName))
                throw new ArgumentNullException("QueryName");
            if (StartDate == null)
                throw new ArgumentNullException("StartDate");
            if (EndDate == null)
                throw new ArgumentNullException("EndDate");
            if (string.IsNullOrEmpty(Dimensions))
                throw new ArgumentNullException("Dimensions");
            if (string.IsNullOrEmpty(Metrics))
                throw new ArgumentNullException("Metrics");

            _generator.SetGenericQueryProperties(StartDate, EndDate);
            _generator.RunQuery(Dimensions, Metrics, PageSize);
            Json = _generator.ResultsAsJsonTable(QueryName);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}