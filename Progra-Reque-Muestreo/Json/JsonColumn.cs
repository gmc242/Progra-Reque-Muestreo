using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAnalytics.Json
{
    public class JsonColumn
    {
        public string ID { set; get; }
        public string Label { set; get; }
        public JsonType Type { set; get; }

        public override string ToString()
        {
            string format = "{id: \'{0}\', label: \'{1}\', type: \'{2}\'}";
            format = format.Replace("{0}", ID);
            format = format.Replace("{1}", Label);
            format = format.Replace("{2}", Type.ToString().ToLower());

            return format;
        }
    }
}