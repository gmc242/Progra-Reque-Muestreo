using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace GoogleAnalytics.Json
{
    public class JsonRow
    {
        public JsonRow()
        {
            Cells = new List<JsonCell>();
        }

        public List<JsonCell> Cells { set; get; }

        public override string ToString()
        {
            StringBuilder rowBldr = new StringBuilder();
            rowBldr.Append("{c:[");
            foreach (JsonCell cell in Cells)
            {
                rowBldr.Append(cell.ToString() + ", ");
            }
            rowBldr = rowBldr.Remove(rowBldr.ToString().LastIndexOf(", "), 2);
            rowBldr.Append("]}");
            return rowBldr.ToString();
        }
    }
}