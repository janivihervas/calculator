using NUnit.Framework;

namespace Calculator
{
    /// @author Jani Viherväs
    /// @version 4.9.2013
    [TestFixture]
    public class StringParseTests
    {
        [Test]
        public void StringToDoubleDecimalPoint()
        {
            Assert.AreEqual(432.4, StringParse.StringToDouble("432.4"));
            Assert.AreEqual(0.0, StringParse.StringToDouble("0.0"));
            Assert.AreEqual(-432.4, StringParse.StringToDouble("-432.4"));
        }

        [Test]
        public void StringToDoubleDecimalComma()
        {
            Assert.AreEqual(432.4, StringParse.StringToDouble("432,4"));
            Assert.AreEqual(0.0, StringParse.StringToDouble("0,0"));
            Assert.AreEqual(-432.4, StringParse.StringToDouble("-432,4"));
        }

    }
}
