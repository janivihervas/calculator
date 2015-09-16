using System;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// Exception class for calculator exceptions
    /// </summary>
    public class CalculatorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="index">The index of the error.</param>
        public CalculatorException(string message, int index)
            : base(message)
        {
            ErrorIndex = index;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <param name="index">The index.</param>
        public CalculatorException(string message, Exception inner, int index)
            : base(message, inner)
        {
            ErrorIndex = index;
        }


        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>
        /// The index of the error.
        /// </value>
        public int ErrorIndex { get; set; }
    }
}
