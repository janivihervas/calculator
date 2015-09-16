using System;
using System.Globalization;
using System.Linq;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Helper class for parsing strings
    /// </summary>
    public class StringParse
    {
        /// <summary>
        /// Strings to double.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Double parsed from the string</returns>
        public static double StringToDouble(string input)
        {
            return input.Contains(".") 
                ? Convert.ToDouble(input, CultureInfo.InvariantCulture) 
                : Convert.ToDouble(input);
        }

        /// <summary>
        /// Finds the needle string in the beginning of the haystack string
        /// </summary>
        /// <param name="haystack">Where to look</param>
        /// <param name="needle">What to look</param>
        /// <param name="ignoreCase">Whether to ignore case</param>
        /// <returns>True if needle was in the start of the haystack</returns>
        public static bool StringStartsWith(string haystack, string needle, bool ignoreCase = false)
        {
            if (!ignoreCase)
            {
                return haystack.StartsWith(needle);
            }

            var input = haystack.ToLower();
            needle = needle.ToLower();

            return input.StartsWith(needle);
        }

        /// <summary>
        /// Finds the needles in the beginning of the haystack string
        /// </summary>
        /// <param name="haystack">The haystack.</param>
        /// <param name="needles">The needles.</param>
        /// <param name="ignoreCase">Whether to ignore case</param>
        /// <returns>Found needle or null</returns>
        public static string StringStartsWith(string haystack, string[] needles, bool ignoreCase = false)
        {
            return needles.FirstOrDefault(needle => StringStartsWith(haystack, needle, ignoreCase));
        }
    }
}
