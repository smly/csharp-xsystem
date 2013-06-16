using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace xsystem
{
    [TestFixture]
    public class TestLittleEndianBitConverter
    {
        [TestFixtureSetUp]
        public void classInit()
        {
        }
        [TestFixtureTearDown]
        public void classCleanUp()
        {
        }
        [SetUp]
        public void init()
        {
        }
        [TearDown]
        public void cleanUp()
        {
        }

        [Test]
        public void testToInt12()
        {
            byte[] value = { 9, 0, 0, 0 };
            int actual, expect;

            actual = LittleEndianBitConverter.ToInt12(value, 0);
            expect = 9;
            Assert.AreEqual(actual, expect);

            actual = LittleEndianBitConverter.ToInt12(value, 1);
            expect = 0;
            Assert.AreEqual(actual, expect);
        }
    }
}