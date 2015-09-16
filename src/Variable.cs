using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Represents a variable with a name and a double value
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// The variables
        /// </summary>
        private static List<Variable> _variables;

        /// <summary>
        /// The constant e
        /// </summary>
        private static Variable _e;

        /// <summary>
        /// The constant pi
        /// </summary>
        private static Variable _pi;

        /// <summary>
        /// The previous answer
        /// </summary>
        private static Variable _ans;

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        /// <value>
        /// The name of the variable.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets and sets the value of the variable.
        /// </summary>
        /// <value>
        /// The value of the variable.
        /// </value>
        public double Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public Variable(string name, double value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="CalculatorException">Value to set was not in correct format or was too big</exception>
        public Variable(string name, string value, int index)
        {
            Name = name;
            try
            {
                Value = StringParse.StringToDouble(value);
            }
            catch (FormatException e)
            {
                throw new CalculatorException("Value to set was not in correct format.", e, index);
            }
            catch ( OverflowException e )
            {
                throw new CalculatorException("Value to set was too big.", e, index);
            }
        }

        /// <summary>
        /// Returns "name = value"
        /// </summary>
        /// <returns>
        /// "name = value"
        /// </returns>
        public override string ToString()
        {
            return Name + " = " + Value;
        }

        /// <summary>
        /// Gets the variables.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        public static List<Variable> Variables
        {
            get
            {
                if (_variables == null || _variables.Count < 3)
                {
                    _variables = new List<Variable> {E, PI, ANS};
                }
                return _variables;
            }
        }

        /// <summary>
        /// Gets constant e (Neper)
        /// </summary>
        public static Variable E
        {
            get { return _e ?? (_e = new Variable("e", Math.E)); }
        }

        /// <summary>
        /// Gets constant pi
        /// </summary>
        public static Variable PI
        {
            get { return _pi ?? (_pi = new Variable("pi", Math.PI)); }
        }

        /// <summary>
        /// Gets the previous answer to calculation
        /// </summary>
        public static Variable ANS
        {
            get { return _ans ?? (_ans = new Variable("ans", 0)); }
        }

        /// <summary>
        /// Gets the variable.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Variable or null if not found</returns>
        public static Variable GetVariable(string name)
        {
            return Variables.FirstOrDefault(variable => variable.Name == name);
        }

        /// <summary>
        /// Gets the variable.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>Variable or null if not found</returns>
        public static Variable GetVariable(string input, int startIndex)
        {
            //foreach ( var variable in Variables )
            //{
            //    if ( (input.Length - startIndex) < variable.Name.Length )
            //    {
            //        continue;
            //    }
            //    var subString = input.Substring(startIndex, variable.Name.Length);
            //    if ( variable.Name == subString )
            //    {
            //        return variable;
            //    }
            //}
            //return null;
            return (from variable in Variables where (input.Length - startIndex) >= variable.Name.Length let subString = input.Substring(startIndex, variable.Name.Length) where variable.Name == subString select variable).FirstOrDefault();
        }

        /// <summary>
        /// Gets the saved variables' names.
        /// </summary>
        /// <returns>Variables' names</returns>
        public static string[] SavedVariables()
        {
            var result = new string[Variables.Count];
            for (var i = 0; i < Variables.Count; i++)
            {
                result[i] = Variables[i].Name;
            }
            return result;
        }
    }
}
