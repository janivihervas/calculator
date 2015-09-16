using System;
using NUnit.Framework;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    [TestFixture]
    public class CalculateTests
    {
        private Calculate _calculate;

        [SetUp]
        protected void SetUp()
        {
            _calculate = new Calculate();
        }

        [Test]
        public void TestDoubleToFraction()
        {
            // This is so that we don't have results where we expect e.g. "2/4" when it actually deducts to "1/2"
            // Remove lines to make tests run faster
            var primes = new[]
            {
                2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71,
                73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
                179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281
                //283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409,
                //419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541
            };
            foreach ( var prime in primes )
            {
                for ( var i = 1; i < prime; i++ )
                {
                    Assert.AreEqual(String.Format("{0}/{1}", i, prime), Calculate.DoubleToFraction(((double)i / prime)));
                }
            }

            for ( var k = 1; k <= 5; k++ )
            {
                foreach ( var prime in primes )
                {
                    for ( var i = 1; i < prime; i++ )
                    {
                        Assert.AreEqual(String.Format("{0} {1}/{2}", k, i, prime), Calculate.DoubleToFraction(((double)i / prime + k)));
                    }
                }
            }
            Assert.AreEqual("3", Calculate.DoubleToFraction(Math.PI));
            Assert.AreEqual("1", Calculate.DoubleToFraction(Math.Sqrt(2)));
            Assert.AreEqual("511/1000", Calculate.DoubleToFraction(0.511));
            Assert.AreEqual("5001/10000", Calculate.DoubleToFraction(0.5001));
        }

        [Test] 
        public void TestFraction()
        {
            var inputHandler = new InputHandler();
            Assert.AreEqual("511/1000", inputHandler.HandleInput("frac 511/1000"));
            Assert.AreEqual("611/1000", inputHandler.HandleInput("frac 511/1000 + 1/10"));
            Assert.AreEqual("3/4", inputHandler.HandleInput("frac 0,25 + 0,5"));
            Assert.AreEqual("1 3/4", inputHandler.HandleInput("frac 0,25 + 1,5"));
        }

        [Test]
        public void CalculateWorksWithOrWithoutSpacesOrCommand()
        {
            Assert.AreEqual("3", _calculate.HandleInput("1+2"));
            Assert.AreEqual("3", _calculate.HandleInput("   1   +   2    "));
            Assert.AreEqual("3", _calculate.HandleInput("calc1+2"));
            Assert.AreEqual("3", _calculate.HandleInput("calc   1   +   2    "));
        }

        [Test]
        public void TestAddition()
        {
            Assert.AreEqual("3", _calculate.HandleInput("1+2"));
            Assert.AreEqual("4,6", _calculate.HandleInput("3.4 + 1.2"));
            Assert.AreEqual("4,6", _calculate.HandleInput("3,4 + 1,2"));
            Assert.AreEqual("6,6", _calculate.HandleInput("3,4 + 1,2+0+2"));
            Assert.AreEqual("6,6", _calculate.HandleInput("ans"));
            Assert.AreEqual("-0,2", _calculate.HandleInput("-3,4 + 1,2+0+2"));
        }

        [Test]
        public void TestSubtraction()
        {
            Assert.AreEqual("1", _calculate.HandleInput("2-1"));
            Assert.AreEqual("2,2", _calculate.HandleInput("3.4 - 1.2"));
            Assert.AreEqual("2,2", _calculate.HandleInput("3,4 - 1,2"));
            Assert.AreEqual("0,2", _calculate.HandleInput("3,4 - 1,2-0 - 2"));
            Assert.AreEqual("0,2", _calculate.HandleInput("ans"));
            Assert.AreEqual("-6,6", _calculate.HandleInput("-3,4 - 1,2 -0-2"));
        }

        [Test]
        public void TestDivision()
        {
            Assert.AreEqual("2", _calculate.HandleInput("2/1"));
            Assert.AreEqual("INF", _calculate.HandleInput("2/0"));
            Assert.AreEqual("INF", _calculate.HandleInput("ans"));
            Assert.AreEqual("-2", _calculate.HandleInput("2/-1"));
            Assert.AreEqual("3", _calculate.HandleInput("3.6 / 1.2"));
            Assert.AreEqual("3", _calculate.HandleInput("ans"));
            Assert.AreEqual("1,5", _calculate.HandleInput("3/2"));
            Assert.AreEqual("1", _calculate.HandleInput("(4/2)/2"));
        }

        [Test]
        public void TestMultiplication()
        {
            Assert.AreEqual("2", _calculate.HandleInput("2*1"));
            Assert.AreEqual("2", _calculate.HandleInput("-2*-1"));
            Assert.AreEqual("0", _calculate.HandleInput("2*0"));
            Assert.AreEqual("4,08", _calculate.HandleInput("3.4 * 1.2"));
            Assert.AreEqual("-4,08", _calculate.HandleInput("-3,4 * 1,2"));
            Assert.AreEqual("8,16", _calculate.HandleInput("3,4 * 1,2 * 2"));
            Assert.AreEqual("-8,16", _calculate.HandleInput("-3,4 * 1,2 *2"));
            Assert.AreEqual("-8,16", _calculate.HandleInput("ans"));
        }

        [Test]
        public void TestPower()
        {
            Assert.AreEqual("2", _calculate.HandleInput("2^1"));
            Assert.AreEqual("1", _calculate.HandleInput("2^0"));
            Assert.AreEqual("8", _calculate.HandleInput("2 ^ 3"));
            Assert.AreEqual("0,125", _calculate.HandleInput("2 ^ -3"));
            Assert.AreEqual("0,125", _calculate.HandleInput("ans"));
            Assert.AreEqual("-8", _calculate.HandleInput("-2 ^ 3"));
            Assert.AreEqual("16", _calculate.HandleInput("-2 ^ 4"));
        }

        [Test]
        public void TestRoot()
        {
            Assert.AreEqual("2", _calculate.HandleInput("root 4"));
            Assert.AreEqual("5", _calculate.HandleInput("root 25"));
            Assert.AreEqual("5", _calculate.HandleInput("ans"));
            Assert.AreEqual("5", _calculate.HandleInput("root (5^2)"));
            Assert.AreEqual("2", _calculate.HandleInput("root [3] 8"));
        }

        [Test]
        public void TestLogarithm()
        {
            Assert.AreEqual("1", _calculate.HandleInput("log e"));
            Assert.AreEqual("1", _calculate.HandleInput("log[2] 2"));
            Assert.AreEqual("1", _calculate.HandleInput("log[10] 10"));
            Assert.AreEqual("0", _calculate.HandleInput("log 1"));
            Assert.AreEqual("-1", _calculate.HandleInput("log[2] (1/2)"));
            Assert.AreEqual("-1", _calculate.HandleInput("ans"));
            Assert.AreEqual("-1", _calculate.HandleInput("log[2] 0,5"));
            Assert.AreEqual("-1", _calculate.HandleInput("log[10] (1/10)"));
            Assert.AreEqual("-2", _calculate.HandleInput("log[10] (1/100)"));
            Assert.AreEqual("-3", _calculate.HandleInput("log[10] (1/1000)"));
        }

        [Test]
        public void TestParenthesis()
        {
            Assert.AreEqual("4", _calculate.HandleInput("2*(1+1)"));
            Assert.AreEqual("-13", _calculate.HandleInput("(2*(-2*3))-1"));
            Assert.AreEqual("0", _calculate.HandleInput("2*(2-2)"));
            Assert.AreEqual("0,5", _calculate.HandleInput("2/(2+2)"));

            Assert.AreEqual("8", _calculate.HandleInput("2^(2+1)"));
            Assert.AreEqual("5", _calculate.HandleInput("log (e^5)"));
            Assert.AreEqual("5", _calculate.HandleInput("root (5^2)"));

            Assert.AreEqual("1", _calculate.HandleInput("root[3]( (1 + root 4 ^ 2) / 5 - 1 + 3 * log e - (-1^2 + 2 )) - log[10] (1/10)"));
            Assert.AreEqual("1", _calculate.HandleInput("ans"));
        }

        [Test]
        public void TestThrowsExceptionOnBadInput()
        {
            // Undefined result
            Assert.Throws<CalculatorException>(() =>_calculate.HandleInput("log [-1] e"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("log [1] e"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("log [0] e"));

            // Undefined result
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("root (-2)"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("root[0] 2"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("root[-1] 2"));

            // Undefined commands / characters
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("roost 2"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput(".root 2"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("root 2,,"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("root[(2+3-4)*5] (5^5)"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("log[(2+3-4)*5] (5^5)"));

            // Undefined variables
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("A"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("ANS"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("E"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("PI"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("log[10] (1/1000) + a"));

            // Missing / too many paranthesis (missing closing paranthesis still passes)
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("(log[10] (1/1000)"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("log[10] (1/1000"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("log10] (1/1000)"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("log[10 (1/1000)"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("root10] (1/1000)"));
            Assert.Throws<CalculatorException>(() => _calculate.HandleInput("root[10 (1/1000)"));
        }

    }
}
