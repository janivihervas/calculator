using System;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Class to handle fraction commands
    /// </summary>
    public class Fraction : CommandBase
    {
        /// <summary>
        /// The command
        /// </summary>
        public new const string Command = "frac";

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="initialInput">The initial input.</param>
        /// <returns></returns>
        public override string HandleInput(string initialInput)
        {
            var calculate = new Calculate();
            var index = 0;

            if ( StringParse.StringStartsWith(initialInput, Command) )
            {
                index = Command.Length;
            }

            try
            {
                calculate.HandleInput(initialInput.Substring(index));
            }
            catch (CalculatorException e)
            {
                e.ErrorIndex += index;
                throw e;
            }
            return Calculate.DoubleToFraction(Variable.ANS.Value);
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <returns></returns>
        public new static string GetHelpText()
        {
            return String.Format("\t-{0}: Print the expression value as fraction number.", Command);
        }

        /// <summary>
        /// Gets the additional help text.
        /// </summary>
        /// <returns></returns>
        public new static string GetAdditionalHelpText()
        {
            return String.Format("Print the expression value as fraction. \n\n" +
                                 "Examples:\n" +
                                 "\t{1} x = 0,1\n" +
                                 "\t{0} x\n" +
                                 "\t=> 1/10\n\n" +
                                 "\t{0} 0,1 * 6,0 / 10\n" +
                                 "\t=> 6/100",
                                 Command, SetVariable.Command);
        }
    }
}
