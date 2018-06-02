<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="report.ascx.cs" Inherits="GoogleAnalytics.Controls.report" %>

<script type="text/javascript">

    //timeline
    function <%= QueryName%>_render() {
        // Create and populate the data table.
        var <%= QueryName%>_data = new google.visualization.DataTable(<%= QueryName%>_setData(), 0.5);
        // Create and draw the visualization.
        visualization = new google.visualization.<%= ChartType %>(document.getElementById('report_<%= QueryName %>'));
        visualization.draw(<%= QueryName%>_data, <%= QueryName%>_options);
    }
    var <%= QueryName%>_options = <%= ChartOptions.ToString() %>
    function <%= QueryName%>_setData() {
        <%= Json.ToString() %>
        return <%= QueryName%>_JsonTable;
    }
    
    Sys.Application.add_load(function() {
        reportManager.addChart(<%= QueryName%>_render);
    });

</script>

<div id="report_<%= QueryName %>"></div>
