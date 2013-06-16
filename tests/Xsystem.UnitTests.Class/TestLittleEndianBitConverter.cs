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
        public void testToInt8()
        {
            byte[] value = { 9, 0, 11, 0 };
            int actual, expect;

            actual = LittleEndianBitConverter.ToInt8(value, 0);
            expect = 9;
            Assert.AreEqual(actual, expect);

            actual = LittleEndianBitConverter.ToInt8(value, 2);
            expect = 11;
            Assert.AreEqual(actual, expect);
        }

        [Test]
        public void testToInt16()
        {
            byte[] value = { 9, 0, 0, 0, 11, 0, 0, 0 };
            int actual, expect;

            actual = LittleEndianBitConverter.ToInt16(value, 0);
            expect = 9;
            Assert.AreEqual(actual, expect);

            actual = LittleEndianBitConverter.ToInt16(value, 4);
            expect = 11;
            Assert.AreEqual(actual, expect);
        }

        [Test]
        public void testToInt12()
        {
            byte[] value = { 9, 0, 0, 11, 0, 0 };
            int actual, expect;

            actual = LittleEndianBitConverter.ToInt12(value, 0);
            expect = 9;
            Assert.AreEqual(actual, expect);

            actual = LittleEndianBitConverter.ToInt12(value, 3);
            expect = 11;
            Assert.AreEqual(actual, expect);
        }
    }
}