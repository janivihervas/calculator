using System;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Base class for command functions. Workaround to get derived classes to "override" static methods
    /// </summary>
    public class CommandBase
    {
        /// <summary>
        /// The command
        /// </summary>
        public const string Command = "command";

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Extending class must override HandleInput method</exception>
        public virtual string HandleInput(string input)
        {
            throw new NotImplementedException("Extending class must override HandleInput method");
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Extending class must hide GetHelpText method</exception>
        public static string GetHelpText()
        {
            throw new NotImplementedException("Extending class must hide GetHelpText method");
        }

        /// <summary>
        /// Gets the additional help text.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Extending class must hide GetAdditionalHelpText method</exception>
        public static string GetAdditionalHelpText()
        {
            throw new NotImplementedException("Extending class must hide GetAdditionalHelpText method");
        }
    }
}
