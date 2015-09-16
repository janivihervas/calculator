using System;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    ///
    /// <summary>
    /// A calculator program, which can calculate simple arithmetic calculations
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main program for starting the calculator
        /// </summary>
        /// <param name="args">Not used</param>
        public static void Main(string[] args)
        {
            var inputHandler = new InputHandler();
            Console.WriteLine(inputHandler.GetStartHelp());
            while ( true )
            {
                var readLine = Console.ReadLine();
                if ( StringParse.StringStartsWith(readLine, inputHandler.GetQuitCommands(), true) != null )
                {
                    Console.WriteLine("Goodbye!\n");
                    break;
                }
                Console.WriteLine(inputHandler.HandleInput(readLine) + "\n");
            }
        }
    }
}
