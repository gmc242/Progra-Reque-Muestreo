using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace GoogleAnalytics.Json
{
    public class JsonTable
    {
        public JsonTable(string uniqueID)
        {
            UniqueID = uniqueID;
            Columns = new List<JsonColumn>();
            Rows = new List<JsonRow>();
        }
        public string UniqueID { set; get; }
        public string ClientKey { get { return UniqueID + "_JsonTable"; } }
        public List<JsonColumn> Columns { set; get; }
        public List<JsonRow> Rows { set; get; }

        public override string ToString()
        {
            if (Rows.Count == 0)
                throw new InvalidOperationException("There are no results for the selected criteria");
            if (Rows.Any(r => r.Cells.Count != Rows[0].Cells.Count))
                throw new ArgumentException("The number of cells in each row must be identical");
            if (Columns.Count != Rows[0].Cells.Count)
                throw new ArgumentException("The number of columns must be equal to the number of cells in each row");
            string formatString = "var #varName# = { cols: [#cols#], rows: [#rows#]};";
            formatString = formatString.Replace("#varName#", ClientKey);
            StringBuilder colsBuilder = new StringBuilder();
            foreach (JsonColumn col in Columns)
            {
                colsBuilder.Append(col.ToString() + ", ");
            }
            colsBuilder = colsBuilder.Remove(colsBuilder.ToString().LastIndexOf(", "), 2);
            formatString = formatString.Replace("#cols#", colsBuilder.ToString());
            StringBuilder rowsBuilder = new StringBuilder();
            foreach(JsonRow row in Rows)
            {
                rowsBuilder.Append(row.ToString() + ", ");
            }
            rowsBuilder = rowsBuilder.Remove(rowsBuilder.ToString().LastIndexOf(", "), 2);
            formatString = formatString.Replace("#rows#", rowsBuilder.ToString());
            return formatString;
        }
    }
}

/*

var JSONObject = {
            cols: [{id: 'task', label: 'Task', type: 'string'},
                {id: 'hours', label: 'Hours per Day', type: 'number'}],
            rows: [{c:[{v: 'Work'}, {v: 11}]},
                {c:[{v: 'Eat'}, {v: 2}]},
                {c:[{v: 'Commute'}, {v: 2, f: '2.000'}]}]};


*/