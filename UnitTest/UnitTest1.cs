using System;
using Home1_testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly MarsColony marsColony = new MarsColony();
        [TestMethod]
        public void RemoteSettersWithOneArg1()
        {
            Assert.AreEqual<int>(0, marsColony.RemoteSetters(10));
        }
        [TestMethod]
        public void RemoteSettersWithOneArg2()
        {
            Assert.AreEqual<int>(-1, marsColony.RemoteSetters(100));
        }
        [TestMethod]
        public void RemoteSettersWithTwoArgs1()
        {
            Type type = typeof(Miner);
            Assert.AreEqual<int>(-1, marsColony.RemoteSetters(10, type));
        }
        [TestMethod]
        public void RemoteSettersWithTwoArgs2()
        {
            Type type = typeof(Robot);
            Assert.AreEqual<int>(9, marsColony.RemoteSetters(1, type));
        }
        [TestMethod]
        public void RemoteSettersWithTwoArgs3()
        {
            Type type = typeof(Unit);
            Assert.AreEqual<int>(-1, marsColony.RemoteSetters(1, type));
        }
        [TestMethod]
        public void CalculateProd1()
        {
            Assert.AreEqual<double>(12.1, marsColony.CalculateProd(marsColony.units, 1));
        }
        [TestMethod]
        public void CalculateProd2()
        {
            Assert.AreEqual<double>(-1, marsColony.CalculateProd(null, 1));
        }
        [TestMethod]
        public void GetColonySetters()
        {
            Assert.AreEqual(marsColony.units, marsColony.GetColonySetters());
        }
    }
}
