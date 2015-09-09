using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VqSoft.Framework.Common.Utils
{
    public sealed class UrlHelperVq
    {
        private UrlHelperVq() { }

        /// <summary>
        /// To check if the string contains query string, e.g: "http:www.site.com?a=b"
        /// </summary>
        private static readonly Regex REG_IS_CONTAIN_QUERY = new Regex(@"^.*\?.+=.+$");

        /// <summary>
        /// Check the string  is like "?a=b", "aaa?c=d"
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static bool IsContainQuery(string stringValue)
        {
            return REG_IS_CONTAIN_QUERY.IsMatch(stringValue);
        }

    }//class
}
