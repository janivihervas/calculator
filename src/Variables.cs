using System;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 6.9.2013
    ///
    /// <summary>
    /// Class to get information from variables
    /// </summary>
    public class Variables : CommandBase
    {
        /// <summary>
        /// The command
        /// </summary>
        public new const string Command = "variables";

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public override string HandleInput(string input)
        {
            return String.Join(",\n", Variable.Variables);
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <returns></returns>
        public new static string GetHelpText()
        {
            return String.Format("\t-{0}: Prints all saved variables and constants.", Command);
        }

        /// <summary>
        /// Gets the additional help text.
        /// </summary>
        /// <returns></returns>
        public new static string GetAdditionalHelpText()
        {
            return "Prints all saved variables and constants.";
        }
    }
}
