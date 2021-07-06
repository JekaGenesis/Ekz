using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ekz;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCase1()
        {
            vvodZnach vz = new vvodZnach();
            vz.simplexBol();
            Assert.AreEqual(vz.ResultTable[vz.ResultTable.GetLength(0) - 1, 0] , -9,11111111111111);
        }
        [TestMethod]
        public void TestCase2()
        {
            vvodZnach vz = new vvodZnach();
            vz.simplexBol();
            Assert.AreEqual(vz.ResultTable[vz.ResultTable.GetLength(0) - 1, 0] *-1, 9,11111111111111);
        }
        [TestMethod]
        public void TestCase3()
        {
            vvodZnach vz = new vvodZnach();
            vz.simplexBol();
            Assert.AreEqual(vz.ResultTable[0,0], 0.888888888888889);
        }
        [TestMethod]
        public void TestCase4()
        {
            vvodZnach vz = new vvodZnach();
            vz.simplexBol();
            Assert.AreEqual(vz.ResultTable[2, 1], 0);
        }
        [TestMethod]
        public void TestCase5()
        {
            vvodZnach vz = new vvodZnach();
            vz.simplexBol();
            Assert.AreEqual(vz.ResultTable[2, 2], 1);
        }
    }
}
