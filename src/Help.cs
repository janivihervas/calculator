using System;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Class to generate help texts
    /// </summary>
    public class Help : CommandBase
    {
        /// <summary>
        /// The command name
        /// </summary>
        public new const string Command = "help";

        /// <summary>
        /// Initializes a new instance of the <see cref="Help"/> class.
        /// </summary>
        public Help()
        {
            SupportedActions = new[] {Calculate.Command, SetVariable.Command, UnsetVariable.Command, Fraction.Command, Variables.Command, Command};
            QuitCommands = new[] {"quit"};
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>
        /// The supported actions.
        /// </value>
        public string[] SupportedActions { get; private set; }

        /// <summary>
        /// Gets the quit commands.
        /// </summary>
        /// <value>
        /// The quit commands.
        /// </value>
        public static string[] QuitCommands { get; private set; }

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="initialInput">The initial input.</param>
        /// <returns></returns>
        public override string HandleInput(string initialInput)
        {
            var trimmed = initialInput.Replace(" ", "").Substring(Command.Length);
            var result = StringParse.StringStartsWith(trimmed, SupportedActions);
            switch ( result )
            {
                case Calculate.Command:
                    return Calculate.GetAdditionalHelpText();
                case SetVariable.Command:
                    return SetVariable.GetAdditionalHelpText();
                case UnsetVariable.Command:
                    return UnsetVariable.GetAdditionalHelpText();
                case Fraction.Command:
                    return Fraction.GetAdditionalHelpText();
                case Variables.Command:
                    return Variables.GetAdditionalHelpText();
                default:
                    return GetHelpText();
            }
        }

        /// <summary>
        /// Generates the help text.
        /// </summary>
        /// <returns>Help text</returns>
        public static new string GetHelpText()
        {
            return "Program: \tCalculator which can calculate simple arithmetic operations.\n" +
                   "Author: \tJani Viherväs\n" +
                   "Version date: \t6.9.2013\n\n" +
                   "Commands:\n" +
                   GetAdditionalHelpText() + "\n" + 
                   Calculate.GetHelpText() + "\n" + 
                   SetVariable.GetHelpText() + "\n" +
                   UnsetVariable.GetHelpText() + "\n" +
                   Variables.GetHelpText() + "\n" + 
                   Fraction.GetHelpText() + "\n" + 
                   GetQuitCommandsHelpText();
        }

        /// <summary>
        /// Gets the additional help text.
        /// </summary>
        /// <returns></returns>
        public static new string GetAdditionalHelpText()
        {
            return String.Format("\t-{0} [COMMAND]: Open these options.\n" +
                                 "\t Use '{0} [COMMAND]' to get additional help, f.g. '{0} {1}'.", Command, Calculate.Command);
        }

        /// <summary>
        /// Generates the start help.
        /// </summary>
        /// <returns></returns>
        public static string GenerateStartHelp()
        {
            return String.Format("Type '{0}' for help options or '{1}' to close the program.\n", Command, String.Join("' or '", QuitCommands));
        }

        /// <summary>
        /// Gets the help text for quit commands
        /// </summary>
        /// <returns></returns>
        private static string GetQuitCommandsHelpText()
        {
            return String.Format("\t-{0}: Quit the program.", String.Join(" or ", QuitCommands));
        }

    }
}
