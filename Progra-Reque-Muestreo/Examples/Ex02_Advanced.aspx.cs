using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoogleAnalytics.Examples
{
    public partial class Ex02_Advanced : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var baseOptions = new Dictionary<string, object>();
            baseOptions.Add("height", 400);
            baseOptions.Add("width", 950);
            baseOptions.Add("is3D", true);

            report_Timeline.QueryName = "Timeline";
            report_Timeline.StartDate = DateTime.Now.AddMonths(-1);
            report_Timeline.EndDate = DateTime.Now;
            report_Timeline.Dimensions = "ga:date";
            report_Timeline.Metrics = "ga:visitors, ga:newVisits, ga:bounces";
            report_Timeline.ChartType = ChartType.LineChart;
            report_Timeline.ChartOptions = new Json.ChartOptions();
            report_Timeline.ChartOptions.Options.Add("title", "Recent Visits");
            report_Timeline.ChartOptions.Extend(baseOptions);
            report_Timeline.DataBind();

            report_Entrances.QueryName = "Entrances";
            report_Entrances.StartDate = DateTime.Now.AddMonths(-1);
            report_Entrances.EndDate = DateTime.Now;
            report_Entrances.Dimensions = "ga:landingPagePath";
            report_Entrances.Metrics = "ga:entrances, ga:pageViews";
            report_Entrances.ChartType = ChartType.Table;
            report_Entrances.ChartOptions = new Json.ChartOptions();
            report_Entrances.ChartOptions.Options.Add("title", "Entrance Pages");
            report_Entrances.ChartOptions.Options.Add("page", "enable");
            report_Entrances.ChartOptions.Options.Add("pageSize", 10);
            report_Entrances.ChartOptions.Extend(baseOptions);
            report_Entrances.PageSize = 50;
            report_Entrances.DataBind();

            report_Exits.QueryName = "Exits";
            report_Exits.StartDate = DateTime.Now.AddMonths(-1);
            report_Exits.EndDate = DateTime.Now;
            report_Exits.Dimensions = "ga:exitPagePath";
            report_Exits.Metrics = "ga:exits";
            report_Exits.ChartType = ChartType.Table;
            report_Exits.ChartOptions = new Json.ChartOptions();
            report_Exits.ChartOptions.Options.Add("title", "Exit Pages");
            report_Exits.ChartOptions.Options.Add("page", "enable");
            report_Exits.ChartOptions.Options.Add("pageSize", 10);
            report_Exits.ChartOptions.Extend(baseOptions);
            report_Exits.PageSize = 50;
            report_Exits.DataBind();

        }
    }
}