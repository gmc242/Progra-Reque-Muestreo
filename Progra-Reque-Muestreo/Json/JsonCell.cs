using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAnalytics.Json
{
    public class JsonCell
    {
        public JsonType Type { set; get; }
        public object Value { set; get; }

        public override string ToString()
        {
            string format = "{v: {0}}";
            if (Type == JsonType.STRING)
                format = format.Replace("{0}", "\'" + Value + "\'");
            else
                format = format.Replace("{0}", Value.ToString());
            return format;
        }
    }
}