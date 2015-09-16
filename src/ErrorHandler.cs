using System.Text;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Class to handle error messages
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        /// Generates the error message.
        /// </summary>
        /// <param name="initialInput">The initial input.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>Error message</returns>
        public static string GenerateErrorMessage(string initialInput, CalculatorException exception)
        {
            var result = new StringBuilder();
            for (var i = 0; i < exception.ErrorIndex; i++)
            {
                result.Append(" ");
            }
            result.Append("^\n");
            result.Append("Syntax error: \n");
            result.Append(exception.Message + "\n");
            if (exception.InnerException != null)
            {
                result.Append("Inner error message:\n");
                result.Append(exception.InnerException.Message + "\n");
            }
            return result.ToString();
        }
    }
}
