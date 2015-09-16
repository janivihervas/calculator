using System;
using System.Collections.Generic;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 5.9.2013
    /// 
    /// <summary>
    /// Class to handle unsetting variables
    /// </summary>
    public class UnsetVariable : CommandBase
    {
        private string _initialInput;
        private int _index;
        public new const string Command = "unset";
        private List<string> _variablesToUnset;

        public override string HandleInput(string input)
        {
            _variablesToUnset = new List<string>();
            _initialInput = input;
            _index = 0;
            if ( StringParse.StringStartsWith(_initialInput, Command) )
            {
                _index = Command.Length;
            }
            
            E();
            return _variablesToUnset.Count > 1
                ? "Variables " + String.Join(", ", _variablesToUnset) + " unset."
                : "Variable " + String.Join(", ", _variablesToUnset) + " unset.";

        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <returns></returns>
        public static new string GetHelpText()
        {
            return String.Format("\t-{0}: Unset variables.", Command);
        }

        /// <summary>
        /// Gets the additional help text.
        /// </summary>
        /// <returns></returns>
        public static new string GetAdditionalHelpText()
        {
            return String.Format("Unset a variable. " +
                     "You can unset multiple variables by separating variable names with a comma ',' or with a semicolon ';'. \n\n" +
                     "NOTE! \n" +
                     "\tTo unset variables they must first be initialized with '{1}' command. \n" +
                     "\tType '{2} {1}' for more help on initializing variables.\n\n" +
                     "Examples:\n" +
                     "\t{0} x\n" +
                     "\t{0} x, y\n" +
                     "\t{0} x; y",
                     Command, SetVariable.Command, Help.Command);
        }

        /// <summary>
        /// <para> Production E: </para> 
        /// <para> E -> NE' </para> 
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
                throw new CalculatorException("Was expecting variable name.", _index);
            }

            // E -> NE'
            var variable = N();
            if (variable == null)
            {
                throw new CalculatorException("Variable not found.", _index);
            }

            var name = variable.Name;
            if (Variable.Variables.Remove(variable))
            {
                _variablesToUnset.Add(name);
            }

            E_();
        }

        /// <summary>
        /// <para> Production E': </para> 
        /// <para> E' -> eps </para> 
        /// <para> E' -> PE </para> 
        /// </summary>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private void E_()
        {
            if ( !SkipSpacesAndContinue() )
            {
                // E' -> eps
                return;
            }

            switch (_initialInput[_index])
            {
                case ',':
                case ';':
                    // E'->PE
                    P();
                    E();
                    break;
                default:
                    throw new CalculatorException("Was expecting ',' or ';'.", _index);
            }
        }

        /// <summary>
        /// <para> Production P: </para> 
        /// <para> P -> , </para> 
        /// <para> P -> ; </para> 
        /// </summary>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private void P()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return;
            }

            switch ( _initialInput[_index] )
            {
                case ',':
                case ';':
                    // P -> ,
                    // P -> ;
                    _index++;
                    break;
                default:
                    throw new CalculatorException("Was expecting ',' or ';'.", _index);
            }
        }

        /// <summary>
        /// <para> Production N: </para> 
        /// <para> N -> s </para> 
        /// </summary>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private Variable N()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return null;
            }

            if (!Char.IsLetter(_initialInput[_index]))
            {
                throw new CalculatorException("Was expecting a letter.", _index);
            }

            // N -> s
            var stop = _index;
            for ( var i = _index; i < _initialInput.Length; i++ )
            {
                if ( _initialInput[i] == ' ' || _initialInput[i] == ',' || _initialInput[i] == ';' )
                {
                    break;
                }
                stop++;
            }

            var name = _initialInput.Substring(_index, stop - _index);
            var variable = Variable.GetVariable(name);

            if (variable == null)
            {
                return null;
            }
            
            _index += name.Length;
            return variable;
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
