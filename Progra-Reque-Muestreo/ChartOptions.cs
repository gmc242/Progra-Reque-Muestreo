using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAnalytics.Json
{
    public class ChartOptions
    {
        /// <summary>
        /// All Chart Options for a Google Chart
        /// </summary>
        public Dictionary<string, object> Options;

        /// <summary>
        /// Manages Adding Chart Options and Converting all options to a JSON string with overridden ToString() method
        /// </summary>
        public ChartOptions()
        {
            Options = new Dictionary<string, object>();
        }

        /// <summary>
        /// Extends options with a list of extended options
        /// </summary>
        /// <param name="extendedOpts"></param>
        public void Extend(Dictionary<string, object> extendedOpts)
        {
            foreach (KeyValuePair<string, object> opt in extendedOpts)
            {
                Options.Add(opt.Key, opt.Value);
            }
        }

        /// <summary>
        /// Casts all options into a JSON string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var entries = Options.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries) + "}";
        }
    }
}