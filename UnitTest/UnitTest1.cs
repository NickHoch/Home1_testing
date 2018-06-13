using System;
using Home1_testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly MarsColony marsColony;
        [TestMethod]
        public void RemoteSettersWithOneArg()
        {
            Assert.AreEqual<int>(20, marsColony.RemoteSetters(10));
        }
    }
}
