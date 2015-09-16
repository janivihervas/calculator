using System;
using System.Text;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Class to handle the calculation
    /// </summary>
    public class Calculate : CommandBase
    {
        /// <summary>
        /// The command name
        /// </summary>
        public new const string Command = "calc";

        /// <summary>
        /// The initial input
        /// </summary>
        private string _initialInput;

        /// <summary>
        /// The index
        /// </summary>
        private int _index;

        /// <summary>
        /// The root operation
        /// </summary>
        public const string Root = "root";

        /// <summary>
        /// The log operation
        /// </summary>
        public const string Log = "log";

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="initialInput">The initial input.</param>
        /// <returns>Calculation result</returns>
        public override string HandleInput(string initialInput)
        {
            _initialInput = initialInput;
            _index = 0;
            if ( StringParse.StringStartsWith(_initialInput, Command) )
            {
                _index = Command.Length;
            }
            var result = E();
            Variable.ANS.Value = result;
            return "" + result;
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <returns></returns>
        public static new string GetHelpText()
        {
            return String.Format("\t-[{0}]: Calculate input. Not necessary to type {0}.", Command);
        }

        /// <summary>
        /// Gets the additional help text.
        /// </summary>
        /// <returns></returns>
        public static new string GetAdditionalHelpText()
        {
            return String.Format("Calculate input. Supported operations are:\n" +
                                 "+, -, *, /, ^, root[base] and log[base].\n\n" +
                                 "You can use decimal point or dot. You don't have to define base numbers for\n" +
                                 "root and log operations.\n" +
                                 "Default values for base number for root is 2 (square root) \n" +
                                 "and for log it is e (~ 2,71828, natural logarithm). \n" +
                                 "You can also use pre-defined variables to calculate.\n\n" +
                                 "NOTE! \n" + 
                                 "\tYou must initialize variables with '{1}' command. \n" +
                                 "\tSee '{2} {1}' for more help on initializing variables.\n\n" +
                                 "Examples:\n" +
                                 "\t{0} 1 + 2\n" +
                                 "\t1 + 2\n" +
                                 "\troot[3] 8 \n" +
                                 "\t => 2 \n" +
                                 "\troot 4 \n" +
                                 "\t => 2 \n" +
                                 "\tlog e \n" +
                                 "\t => 1 \n" +
                                 "\tlog[10] 10 \n" +
                                 "\t => 1 \n" +
                                 "\t{1} x = 3\n" +
                                 "\tx + 2\n" +
                                 "\t => 5", 
                                 Command, SetVariable.Command, Help.Command);
        }

        /// <summary>
        /// Converts double numbers to fraction
        /// </summary>
        /// <param name="digit">The digit.</param>
        /// <returns>Fraction of the number</returns>
        public static string DoubleToFraction(double digit)
        {
            var integer = (int)digit;
            var rest = Math.Abs(digit - integer);
            const int limit = 10000;
            const double epsilon = 1E-8;
            var negative = "";

            if (rest < epsilon)
            {
                return "" + integer;
            }

            for (var i = 2; i <= limit; i++)
            {
                for (var j = 1; j < i; j++)
                {
                    if ( (double)j / i > rest + epsilon )
                    {
                        break;
                    }
                    if (Math.Abs((double) j/i - rest) < epsilon)
                    {
                        return integer == 0
                            ? String.Format("{2}{0}/{1}", j, i, negative)
                            : String.Format("{0} {1}/{2}", integer, j, i);
                    }
                }
            }
            return "" + integer;
        }


        #region Grammar and calculations

        /// <summary>
        /// <para> Production E: </para> 
        /// <para> E -> TE' </para> 
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double E()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return 0;
            }
            if ( Char.IsNumber(_initialInput[_index]) || 
                IsRootOrLogCommand() || 
                Variable.GetVariable(_initialInput, _index) != null ||
                _initialInput[_index] == '-' ||
                _initialInput[_index] == '(')
            {
                // E -> TE'
                var n = T();
                return E_(n);
            }

            throw new CalculatorException("Was expecting a number, variable name (" + String.Join(", ", Variable.SavedVariables()) + "), '-', '(', 'root' or 'log'", _index);
        }

        /// <summary>
        /// <para> Production E': </para> 
        /// <para> E' -> eps </para> 
        /// <para> E' -> +TE' </para>
        /// <para> E' -> -TE' </para>
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double E_(double n)
        {
            if ( !SkipSpacesAndContinue() )
            {
                return n;
            }
            switch ( _initialInput[_index] )
            {
                case ')':
                case ';':
                    // E' -> eps
                    return n;
                case '+':
                    // E -> +TE'
                    _index++;
                    var x = T();
                    return E_(n + x);
                case '-':
                    // E -> -TE'
                    _index++;
                    x = T();
                    return E_(n - x);
                default:
                    throw new CalculatorException("Was expecting ';', ')', '+' or '-'.", _index);
            }
        }

        /// <summary>
        /// <para> Production T: </para> 
        /// <para> T -> PT' </para> 
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double T()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return 0;
            }
            if ( Char.IsNumber(_initialInput[_index]) ||
                Variable.GetVariable(_initialInput, _index) != null ||
                _initialInput[_index] == '(' ||
                _initialInput[_index] == '-' || 
                IsRootOrLogCommand())
            {
                // T -> PT'
                var n = P();
                return T_(n);
            }

            throw new CalculatorException("Was expecting a number, variable name (" + String.Join(", ", Variable.SavedVariables()) + "), '-', '(', 'root' or 'log'", _index);
        }

        /// <summary>
        /// <para> Production T': </para> 
        /// <para> T' -> eps </para> 
        /// <para> T' -> *PT' </para>
        /// <para> T' -> /PT' </para>
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double T_(double n)
        {
            if ( !SkipSpacesAndContinue() )
            {
                return n;
            }
            switch (_initialInput[_index])
            {
                case ')':
                case '+':
                case '-':
                case ';':
                    // T' -> eps
                    return n;
                case '*':
                    // T' -> *PT'
                    _index++;
                    var x = P();
                    return T_(n * x);
                case '/':
                    // T' -> /PT'
                    _index++;
                    x = P();
                    return T_(n / x);
                default:
                    throw new CalculatorException("Was expecting ';', ')', '+', '-', '*' or '/'.", _index);
            }
        }

        /// <summary>
        /// <para> Production P: </para> 
        /// <para> P -> FP' </para> 
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double P()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return 0;
            }
            if ( Char.IsNumber(_initialInput[_index]) ||
                Variable.GetVariable(_initialInput, _index) != null ||
                _initialInput[_index] == '(' ||
                _initialInput[_index] == '-' ||
                IsRootOrLogCommand() )
            {
                // P -> FP'
                var n = F();
                return P_(n);
            }

            throw new CalculatorException("Was expecting a number, variable name (" + String.Join(", ", Variable.SavedVariables()) + "), '-', '(', 'root' or 'log'", _index);
        }

        /// <summary>
        /// <para> Production P': </para> 
        /// <para> P' -> eps </para> 
        /// <para> P' -> ^FP' </para>
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double P_(double n)
        {
            if ( !SkipSpacesAndContinue() )
            {
                return n;
            }
            switch ( _initialInput[_index] )
            {
                case ')':
                case '+':
                case '-':
                case '*':
                case '/':
                case ';':
                    // P' -> eps
                    return n;
                case '^':
                    // P' -> ^FP'
                    _index++;
                    var x = F();
                    return P_(Math.Pow(n, x));
                default:
                    throw new CalculatorException("Was expecting ';', ')', '+', '-', '*', '/' or '^'", _index);
            }
        }

        /// <summary>
        /// <para> Production F: </para> 
        /// <para> F -> C </para> 
        /// <para> F -> -F </para>
        /// <para> F -> root F' F </para>
        /// <para> F -> log F' F </para>
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double F()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return 0;
            }
            if ( Char.IsNumber(_initialInput[_index]) ||
                Variable.GetVariable(_initialInput, _index) != null ||
                _initialInput[_index] == '(')
            {
                // F -> C
                return C();
            }
            if ( _initialInput[_index] == '-')
            {
                // F -> -F
                _index++;
                return -1*F();
            }
            if ( IsRootCommand() )
            {
                // F -> root F' F
                _index += Root.Length;
                var x = F_();
                var y = F();
                if (x == -1.0)
                {
                    if (y < 0)
                    {
                        throw new CalculatorException("Calculation of imaginary numbers is not supported.", _index);
                    }
                    return Math.Sqrt(y);
                }
                if (x == 0.0)
                {
                    throw new CalculatorException("0th root is undefined.", _index);
                }
                
                return Math.Pow(y, 1 / x);
            }
            if (IsLogCommand())
            {
                // F -> log F' F
                _index += Log.Length;
                var x = F_();
                var y = F();
                if (x == -1.0)
                {
                    if ( y <= 0 )
                    {
                        throw new CalculatorException("Logarithm of a number of lesser or equal of 0 is undefined.", _index);
                    }
                    return Math.Log(y);
                }
                if (x <= 0 || x == 1.0)
                {
                    throw new CalculatorException("Base of logarithm must be higher than 0 and not 1.", _index);
                }
                if ( y <= 0 )
                {
                    throw new CalculatorException("Logarithm of a number of lesser or equal of 0 is undefined.", _index);
                }
                return Math.Log(y, x);
            }
            throw new CalculatorException("Was expecting a number, variable name (" + String.Join(", ", Variable.SavedVariables()) + "), '(', '-', 'root' or 'log'.", _index);
        }

        /// <summary>
        /// <para> Production F': </para> 
        /// <para> F' -> eps </para> 
        /// <para> F' -> [C] </para>
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double F_()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return -1;
            }
            switch ( _initialInput[_index] )
            {
                case '(':
                case ')':
                case '+':
                case '-':
                case '*':
                case '/':
                case '^':
                case ';':
                    // F' -> eps
                    return -1;
            }
            if (Char.IsNumber(_initialInput[_index]) ||
                Variable.GetVariable(_initialInput, _index) != null ||
                IsRootOrLogCommand())
            {
                // F' -> eps
                return -1;
            }
            if (_initialInput[_index] == '[')
            {
                // F' -> [C]
                _index++;
                var n = C();
                // ']'
                if (_initialInput.Length <= _index || _initialInput[_index] != ']')
                {
                    throw new CalculatorException("Was expecting ']'.", _index);
                }
                _index++;
                return n;
            }
            throw new CalculatorException("Was expecting " +
                                          "a number, variable name (" + String.Join(", ", Variable.SavedVariables()) + "), 'root', 'log', " +
                                          "'(', ')', '+', '-', '*', '/' or '^'.", _index);
        }

        /// <summary>
        /// <para> Production C: </para> 
        /// <para> C -> c </para> 
        /// <para> C -> v </para>
        /// <para> C -> (E) </para>
        /// </summary>
        /// <returns>Calculation result</returns>
        /// <exception cref="CalculatorException">On syntax error</exception>
        private double C()
        {
            if ( !SkipSpacesAndContinue() )
            {
                return 0;
            }
            if (Char.IsNumber(_initialInput[_index]))
            {
                // C -> c
                var number = GetNumberStringAndIncreaseIndex();
                double n;
                try
                {
                    n = StringParse.StringToDouble(number);
                }
                catch (FormatException e)
                {
                    throw new CalculatorException("Value was not in correct format.", e, _index);
                }
                catch ( OverflowException e )
                {
                    throw new CalculatorException("Value was too big.", e, _index);
                }
                
                return n;
            }
            var variable = Variable.GetVariable(_initialInput, _index);
            if (variable != null)
            {
                // C -> v
                _index += variable.Name.Length;
                return variable.Value;
            }
            if (_initialInput[_index] == '(')
            {
                // C -> (E)
                _index++;
                var n = E();
                if ( _initialInput.Length <= _index || _initialInput[_index] != ')' )
                {
                    throw new CalculatorException("Was expecting ')'", _index);
                }
                _index++;
                return n;
            }

            throw new CalculatorException("Was expecting a number, variable name (" + String.Join(", ", Variable.SavedVariables()) + " or '('", _index);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets number in the input as string and increases current index.
        /// </summary>
        /// <returns>Number as string</returns>
        private string GetNumberStringAndIncreaseIndex()
        {
            var sb = new StringBuilder(_initialInput.Substring(_index));
            for (var i = 0; i < sb.Length; i++)
            {
                if (Char.IsNumber(_initialInput[_index]) ||
                    _initialInput[_index] == '.' ||
                    _initialInput[_index] == ',')
                {
                    _index++;
                }
                else
                {
                    sb.Remove(i, sb.Length - i);
                    break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Skips the spaces and returns true if can continue.
        /// </summary>
        /// <returns>True if can continue</returns>
        private bool SkipSpacesAndContinue()
        {
            while ( _index < _initialInput.Length)
            {
                if (_initialInput[_index] != ' ')
                {
                    break;
                }
                _index++;
            }
            return _index < _initialInput.Length;
        }

        /// <summary>
        /// Determines whether input at current index is root or log command.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if is root or log command; otherwise, <c>false</c>.
        /// </returns>
        private bool IsRootOrLogCommand()
        {
            return IsRootCommand() || IsLogCommand();
        }

        /// <summary>
        /// Determines whether input at current index is root command.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if is root command; otherwise, <c>false</c>.
        /// </returns>
        private bool IsRootCommand()
        {
            try
            {
                var sub = _initialInput.Substring(_index, Root.Length);
                return sub == Root;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether input at current index is log command.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if is log command; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLogCommand()
        {
            try
            {
                var sub = _initialInput.Substring(_index, Log.Length);
                return sub == Log;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        #endregion
    }
}
