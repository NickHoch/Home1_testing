using System;
using System.Collections.Generic;
using Home1_testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly MarsColony marsColony = new MarsColony();
        private readonly RobotCreator robotCreator = new RobotCreator();
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
        public void LocateSetters1()
        {
            Assert.AreEqual<int>(12, marsColony.LocateSetters(new List<Unit> { new Miner(), new Cook() }));
        }
        [TestMethod]
        public void LocateSetters2()
        {
            Assert.AreEqual<int>(10, marsColony.LocateSetters(null));
        }
        [TestMethod]
        public void CalculateProd1()
        {
            ICollection<Unit> units = new List<Unit>()
            {
                new Miner() { Gender = true },
                new Cook() { Gender = false }
            };
            Assert.AreEqual<double>(1.5, marsColony.CalculateProd(units, 1));
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
        [TestMethod]
        public void CalculatorPopulations1()
        {
            ICollection<Unit> units = new List<Unit>()
            {
                new Miner() { Gender = true },
                new Cook() { Gender = false }
            };
            Assert.AreEqual(3, marsColony.CalculatorPopulations(units, 1, new List<Config> { new ConfigColonist { QuantityMale = 1, Type = typeof(Cook) } }));
        }
        [TestMethod]
        public void CalculatorPopulations2()
        {
            ICollection<Unit> units = new List<Unit>()
            {
                new Robot(),
                new Cook()
            };
            Assert.AreEqual(2, marsColony.CalculatorPopulations(units, 1, new List<Config> { new ConfigColonist { QuantityMale = 1, Type = typeof(Cook) } }));
        }
        [TestMethod]
        public void FactoryMethodForRobot1()
        {
            List<Unit> units = new List<Unit>()
            {
                new Robot(),
                new Robot()
            };
            CollectionAssert.AreEqual(units, (robotCreator.FactoryMethod(new ConfigRobot { QuantityInstance = 2 })) as List<Unit>);
            //Assert.AreEqual(2, marsColony.CalculatorPopulations(units, 1, new List<Config> { new ConfigColonist { QuantityMale = 1, Type = typeof(Cook) } }));
        }
        [TestMethod]
        public void FactoryMethodForRobot2()
        {
            ICollection<Unit> units = new List<Unit>()
            {
                new Robot(),
                new Robot()
            };
            Assert.AreEqual(units, robotCreator.FactoryMethod(new ConfigRobot { QuantityInstance = 2 }));
        }
    }
}