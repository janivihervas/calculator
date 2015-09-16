using System;
using System.Collections.Generic;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Class to set variables
    /// </summary>
    public class SetVariable : CommandBase
    {
        /// <summary>
        /// The command name
        /// </summary>
        public new const string Command = "set";

        /// <summary>
        /// The initial input
        /// </summary>
        private string _initialInput;

        /// <summary>
        /// The calculate object to handle calculation expressions when setting variable value
        /// </summary>
        private readonly Calculate _calculate;

        /// <summary>
        /// The index
        /// </summary>
        private int _index;

        /// <summary>
        /// The variables to set
        /// </summary>
        private List<Variable> _variablesToSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetVariable"/> class.
        /// </summary>
        public SetVariable()
        {
            _calculate = new Calculate();
        }

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="initialInput">The initial input.</param>
        /// <returns></returns>
        public override string HandleInput(string initialInput)
        {
            _variablesToSet = new List<Variable>();
            _initialInput = initialInput;
            _index = 0;
            if ( StringParse.StringStartsWith(_initialInput, Command) )
            {
                _index = Command.Length;
            }
            E();
            return String.Join(", ", _variablesToSet);
        }
        
        /// <summary>
        /// <para> Production E: </para> 
        /// <para> E -> N = VE' </para> 
        /// </summary>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private void E()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return;
            }
            if (!Char.IsLetter(_initialInput[_index]))
            {
                throw new CalculatorException("Was expecting a letter.", _index);
            }
            // E -> N = VE'
            var name = N();
            if (name == null ||
                name == Variable.E.Name ||
                name == Variable.PI.Name ||
                name == Variable.ANS.Name )
            {
                throw new CalculatorException("Variable name is reserved for constant value.", _index);
            }
            if (name == Calculate.Root ||
                name == Calculate.Log ||
                name == Command ||
                name == Calculate.Command ||
                name == Help.Command ||
                name == Fraction.Command)
            {
                throw new CalculatorException("Variable name is reserved for existing operation.", _index);
            }

            if ( !SkipSpacesAndContinue() )
            {
                return;
            }

            if (_initialInput[_index] != '=')
            {
                throw new CalculatorException("Was expecting '='.", _index);
            }
            _index++;
            
            var value = V();
            var oldVariable = Variable.GetVariable(name);

            if (oldVariable != null)
            {
                oldVariable.Value = value;
                _variablesToSet.Add(oldVariable);
            }
            else
            {
                var newVariable = new Variable(name, value);
                _variablesToSet.Add(newVariable);
                Variable.Variables.Add(newVariable);
            }
            
            E_();
        }

        /// <summary>
        /// <para> Production E': </para> 
        /// <para> E' -> eps </para> 
        /// <para> E' -> ;E </para> 
        /// </summary>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private void E_()
        {
            if ( !SkipSpacesAndContinue() )
            {
                // E' -> eps
                return;
            }
            if ( _initialInput[_index] == ';' )
            {
                // E' -> ;E
                _index++;
                E();
            }
        }

        /// <summary>
        /// <para> Production N: </para> 
        /// <para> N -> s </para> 
        /// </summary>
        /// <returns>Parsed name</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private string N()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return null;
            }
            if ( !Char.IsLetter(_initialInput[_index]) )
            {
                throw new CalculatorException("Was expecting a letter.", _index);
            }
            // N -> s
            var stop = _index;
            for (var i = _index; i < _initialInput.Length; i++)
            {
                if ( _initialInput[i] == ' ' || _initialInput[i] == '=' )
                {
                    break;
                }
                stop++;
            }
            var name = _initialInput.Substring(_index, stop - _index);
            CheckVariableName(name);
            _index += name.Length;
            return name;
        }

        /// <summary>
        /// Checks that the variable name contains only letters
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <exception cref="CalculatorException">If variable name contains anything else then letters.</exception>
        private void CheckVariableName(string name)
        {
            for (var i = 0; i < name.Length; i++)
            {
                if ( !Char.IsLetter(name[i]) )
                {
                    throw new CalculatorException("Variable names can only contain letters." , _index + i);
                }
            }
        }

        /// <summary>
        /// <para> Production V: </para> 
        /// <para> V -> v, </para> 
        /// <para> where v means value and it is acquired by Calculate-class</para> 
        /// </summary>
        /// <returns>Value</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double V()
        {
            try
            {
                // V -> v
                _calculate.HandleInput(_initialInput.Substring(_index));
            }
            catch (CalculatorException e)
            {
                e.ErrorIndex += _index;
                throw e;
            }
            // At this point our index points at f.g.
            // "set x = root 2 + 4 / 2 - root e; y = 1
            //          ^
            for (var i = _index; i < _initialInput.Length; i++)
            {
                if (_initialInput[i] == ';')
                {
                    break;
                }
                _index++;
            }
            // Last answer is always stored in "ANS" variable
            return Variable.ANS.Value;
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <returns></returns>
        public new static string GetHelpText()
        {
            return String.Format("\t-{0}: Set variable name and value.", Command);
        }

        /// <summary>
        /// Gets the additional help text.
        /// </summary>
        /// <returns></returns>
        public new static string GetAdditionalHelpText()
        {
            return String.Format("Set variable in format 'name = value', and use semicolon to seperate multiple\nvariables. " +
                                 "You can also use calculation expression to initialize variable value. \n\n" +
                                 "Examples:\n" +
                                 "\t{0} x = 2\n" +
                                 "\t{0} x = 5.4 + e * pi - root[3] 8\n" +
                                 "\t{0} x = 34,2; y = 1\n" +
                                 "\t{0} x = x / 2\n" +
                                 "\t => x = 17,1", Command);
        }

        /// <summary>
        /// Skips the spaces and returns true if can continue.
        /// </summary>
        /// <returns>True if can continue</returns>
        private bool SkipSpacesAndContinue()
        {
            while ( _index < _initialInput.Length )
            {
                if ( _initialInput[_index] != ' ' )
                {
                    break;
                }
                _index++;
            }
            return _index < _initialInput.Length;
        }
    }
}
