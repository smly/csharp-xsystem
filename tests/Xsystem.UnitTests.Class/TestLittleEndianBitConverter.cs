using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using XSystem.Class;

namespace XSystem.UnitTest
{
    [TestFixture]
    public class TestLEBitConverter
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
        public void testFrom2ToInt()
        {
            byte[] value = { 9, 0, 11, 0 };
            int actual, expect;

            actual = LEBitConverter.From2ToInt(value, 0);
            expect = 9;
            Assert.AreEqual(actual, expect);

            actual = LEBitConverter.From2ToInt(value, 2);
            expect = 11;
            Assert.AreEqual(actual, expect);
        }

        [Test]
        public void testFrom4ToInt()
        {
            byte[] value = { 9, 0, 0, 0, 11, 0, 0, 0 };
            int actual, expect;

            actual = LEBitConverter.From4ToInt(value, 0);
            expect = 9;
            Assert.AreEqual(actual, expect);

            actual = LEBitConverter.From4ToInt(value, 4);
            expect = 11;
            Assert.AreEqual(actual, expect);
        }

        [Test]
        public void testFrom3ToInt()
        {
            byte[] value = { 9, 0, 0, 11, 0, 0 };
            int actual, expect;

            actual = LEBitConverter.From3ToInt(value, 0);
            expect = 9;
            Assert.AreEqual(actual, expect);

            actual = LEBitConverter.From3ToInt(value, 3);
            expect = 11;
            Assert.AreEqual(actual, expect);
        }
    }
}