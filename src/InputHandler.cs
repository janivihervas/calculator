using System.Linq;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Class to handle all input commands
    /// </summary>
    public class InputHandler
    {
        /// <summary>
        /// The initial input
        /// </summary>
        private string _initialInput;

        /// <summary>
        /// The calculate object
        /// </summary>
        private readonly Calculate _calculate;

        /// <summary>
        /// The set variable object
        /// </summary>
        private readonly SetVariable _setVariable;

        /// <summary>
        /// The help object
        /// </summary>
        private readonly Help _help;

        /// <summary>
        /// The fraction object
        /// </summary>
        private readonly Fraction _fraction;

        /// <summary>
        /// The Unset variable object
        /// </summary>
        private readonly UnsetVariable _unsetVariable;

        /// <summary>
        /// The variables object
        /// </summary>
        private readonly Variables _variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputHandler"/> class.
        /// </summary>
        public InputHandler()
        {
            _fraction = new Fraction();
            _setVariable = new SetVariable();
            _unsetVariable = new UnsetVariable();
            _variables = new Variables();
            _calculate = new Calculate();
            _help = new Help();
        }

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Result message</returns>
        public string HandleInput(string input)
        {
            _initialInput = input;
            var actions = new string [_help.SupportedActions.Length + Variable.Variables.Count];
            for (var i = 0; i < Variable.Variables.Count; i++)
            {
                actions[i] = Variable.Variables[i].Name;
            }
            _help.SupportedActions.CopyTo(actions, Variable.Variables.Count);
            
            var result = StringParse.StringStartsWith(_initialInput, actions.ToArray());
            try
            {
                switch ( result )
                {
                    case Calculate.Command:
                        return _calculate.HandleInput(_initialInput);
                    case SetVariable.Command:
                        return _setVariable.HandleInput(_initialInput);
                    case UnsetVariable.Command:
                        return _unsetVariable.HandleInput(_initialInput);
                    case Help.Command:
                        return _help.HandleInput(_initialInput);
                    case Fraction.Command:
                        return _fraction.HandleInput(_initialInput);
                    case Variables.Command:
                        return _variables.HandleInput(_initialInput);
                    default:
                        return _calculate.HandleInput(_initialInput);
                }
            }
            catch (CalculatorException e)
            {
                return ErrorHandler.GenerateErrorMessage(_initialInput, e);
            }
        }

        /// <summary>
        /// Gets the quit commands.
        /// </summary>
        /// <returns>The quit commands</returns>
        public string[] GetQuitCommands()
        {
            return Help.QuitCommands;
        }

        /// <summary>
        /// Gets the start help.
        /// </summary>
        /// <returns></returns>
        public string GetStartHelp()
        {
            return Help.GenerateStartHelp();
        }
    }
}
