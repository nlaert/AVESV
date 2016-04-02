using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jsonzai.Reflect;

namespace Jsonzai.Test
{
    [TestClass]
    public class NumericTypesTest
    {
        
        [TestMethod]
        public void TestJsonfierIntValues()
        {
            int src = 10;
            string actual = Jsonfier.ToJson(src);
            Assert.AreEqual(src.ToString(), actual);
        }

        [TestMethod]
        public void TestJsonfierDoubleValue()
        {
            double src = 100000.2;
            string actual = Jsonfier.ToJson(src);
            Assert.AreEqual("100000.2", actual);
        }

        [TestMethod]
        public void TestJsonfierFloatValue()
        {
            float src = 100000.2F;
            string actual = Jsonfier.ToJson(src);
            Assert.AreEqual("100000.2", actual);
        }
    }
    
}
