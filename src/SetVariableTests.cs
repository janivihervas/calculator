using NUnit.Framework;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    [TestFixture]
    public class SetVariableTests
    {
        private InputHandler _inputHandler;

        [SetUp]
        protected void SetUp()
        {
            _inputHandler = new InputHandler();
        }

        [TearDown]
        protected void TearDown()
        {
            Variable.Variables.RemoveAll(variable => variable != null);
        }

        [Test]
        public void VariableIsSetAndCanBeUsedAndModified()
        {
            Assert.AreEqual("x = 2", _inputHandler.HandleInput("set x = 2"));
            Assert.AreEqual("x = 2", _inputHandler.HandleInput("set x=2"));

            Assert.AreEqual("2", _inputHandler.HandleInput("x"));

            Assert.AreEqual("6", _inputHandler.HandleInput("x + 4"));

            Assert.AreEqual("x = 4", _inputHandler.HandleInput("set x = x * 2"));

            Assert.AreEqual("2", _inputHandler.HandleInput("root x"));
            Assert.AreEqual("16", _inputHandler.HandleInput("x ^ 2"));
            Assert.AreEqual("x = 16", _inputHandler.HandleInput("set x = ans"));
        }

        [Test]
        public void VariablesAreGlobal()
        {
            Assert.AreEqual("x = 2", _inputHandler.HandleInput("set x = 2"));
            Assert.AreEqual("2", _inputHandler.HandleInput("x"));
            _inputHandler.HandleInput("x + 4");
            Assert.AreEqual("6", _inputHandler.HandleInput("ans"));
            var inputHandler2 = new InputHandler();
            Assert.AreEqual("6", inputHandler2.HandleInput("ans"));
            Assert.AreEqual("2", inputHandler2.HandleInput("x"));
        }

        [Test]
        public void ThrowsExceptionOnBadInput()
        {
            var setVariable = new SetVariable();

            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x2 = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x 2 = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("= 1"));

            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x.y = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x,y = 1"));

            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x-2 = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x+2 = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x/2 = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x*2 = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("x^2 = 1"));

            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("e = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("pi = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("ans = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("root = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("log = 1"));

            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("help = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("frac = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("calc = 1"));
            Assert.Throws<CalculatorException>(() => setVariable.HandleInput("set = 1"));
        }

        [Test]
        public void VariablesCanBeUnset()
        {
            var count = Variable.Variables.Count;
            
            Assert.AreEqual("x = 2", _inputHandler.HandleInput("set x = 2"));
            Assert.AreEqual(count + 1, Variable.Variables.Count);
            
            Assert.AreEqual("Variable x unset.", _inputHandler.HandleInput("unset x"));
            Assert.AreEqual(count, Variable.Variables.Count);
            
            var unsetVariable = new UnsetVariable();
            var calculate = new Calculate();

            Assert.Throws<CalculatorException>(() => unsetVariable.HandleInput("unset x"));
            Assert.Throws<CalculatorException>(() => calculate.HandleInput("x"));
        }

        [Test]
        public void MultipleVariablesCanBeUnsetAtTheSameTime()
        {
            var count = Variable.Variables.Count;
            var unsetVariable = new UnsetVariable();
            var calculate = new Calculate();

            Assert.AreEqual("x = 2", _inputHandler.HandleInput("set x = 2"));
            Assert.AreEqual("y = 4", _inputHandler.HandleInput("set y = 4"));
            Assert.AreEqual("z = 6", _inputHandler.HandleInput("set z = 6"));
            Assert.AreEqual(count + 3, Variable.Variables.Count);

            Assert.AreEqual("Variable x unset.", _inputHandler.HandleInput("unset x"));
            Assert.Throws<CalculatorException>(() => unsetVariable.HandleInput("unset x"));
            Assert.AreEqual(count + 2, Variable.Variables.Count);

            Assert.AreEqual("Variables z, y unset.", _inputHandler.HandleInput("unset z,    y"));
            Assert.AreEqual(count, Variable.Variables.Count);

            Assert.Throws<CalculatorException>(() => unsetVariable.HandleInput("unset y, z"));
            Assert.Throws<CalculatorException>(() => unsetVariable.HandleInput("unset z"));
            Assert.Throws<CalculatorException>(() => calculate.HandleInput("x"));
            Assert.Throws<CalculatorException>(() => calculate.HandleInput("y"));
            Assert.Throws<CalculatorException>(() => calculate.HandleInput("z"));


            Assert.AreEqual("y = 4", _inputHandler.HandleInput("set y = 4"));
            Assert.AreEqual("z = 6", _inputHandler.HandleInput("set z = 6"));
            Assert.AreEqual("Variables y, z unset.", _inputHandler.HandleInput("unset y;  z"));
        }

        [Test]
        public void MultipleVariablesCanBeSetAtTheSameTime()
        {
            var count = Variable.Variables.Count;

            Assert.AreEqual("x = 2, y = 4, z = 6", _inputHandler.HandleInput("set x = 2; y = 4; z = 6"));
            Assert.AreEqual(count + 3, Variable.Variables.Count);

            Assert.AreEqual("2", _inputHandler.HandleInput("x"));
            Assert.AreEqual("4", _inputHandler.HandleInput("y"));
            Assert.AreEqual("6", _inputHandler.HandleInput("z"));
        }

        [Test] 
        public void VariablesCanBeSetWithCalculation()
        {
            Assert.AreEqual("x = 10", _inputHandler.HandleInput("set x = 2 + 4 * root[3]8 * log e"));
            Assert.AreEqual("y = 11", _inputHandler.HandleInput("set y = ans + 1"));

            Assert.AreEqual("xy = 21, z = -20", _inputHandler.HandleInput("set xy = x + y; z = ans * -1 + 1"));
        }
    }
}
