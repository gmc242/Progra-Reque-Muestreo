using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoogleAnalytics.Examples
{
    public partial class Ex01_Simple : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            report_Simple.QueryName = "Simple";
            report_Simple.StartDate = DateTime.Now.AddMonths(-1);
            report_Simple.EndDate = DateTime.Now;
            report_Simple.Dimensions = "ga:date";
            report_Simple.Metrics = "ga:visitors, ga:newVisits, ga:bounces";
            report_Simple.ChartType = ChartType.LineChart;
            report_Simple.ChartOptions = new Json.ChartOptions();
            report_Simple.ChartOptions.Options.Add("title", "This Is My First Chart");
            report_Simple.ChartOptions.Options.Add("height", 400);
            report_Simple.ChartOptions.Options.Add("width", 950);
            report_Simple.ChartOptions.Options.Add("is3D", true);
            report_Simple.DataBind();
        }
    }
}