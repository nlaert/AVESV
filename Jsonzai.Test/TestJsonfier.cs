using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Jsonzai.Test.Model;
using System.Globalization;
using Jsonzai.Reflect;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Jsonzai.Test
{
    [TestClass]
    public class TestJsonfier
    {
        enum Colors { Red=3, Green, Blue, Yellow };
        enum Days : int { Jan, Fev, Mar };


        [TestMethod]
        public void TestEnum1()
        {
             String[] expected = { "Red", "Green", "Blue", "Yellow" };
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(typeof(Colors));
            
            string[] actual = JsonConvert.DeserializeObject<String[]>(json);
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestEnum2()
        {
            string[] expected = {"Jan","Fev","Mar" };
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(typeof(Days));
            //string json = "["\jan\","\Fev\", "\Mar\"]";
            string[] actual = JsonConvert.DeserializeObject<string[]>(json);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJsonfierStudent()
        {
            Student expected = new Student(27721, "Ze Manel");
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(expected, Jsonfier.Selection.Fields);
            //string json = "{\"nr\":27721,\"name\":\"Ze Manel\"}";
            Student actual = JsonConvert.DeserializeObject<Student>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJsonfierArrayPrimitives()
        {
            int[] expected = { 4, 5, 6, 7 };
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(expected, Jsonfier.Selection.Fields);
            //string json = "[4,5,6,7]";
            int[] actual = JsonConvert.DeserializeObject<int[]>(json);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJsonfierCourse()
        {
            Course expected = new Course
            (
                "AVE",
                new Student[4]{
                    new Student(27721, "Ze Manel"),
                    new Student(15642, "Maria Papoila"),
                    null,
                    null
                }
            );
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(expected, Jsonfier.Selection.Fields);
            //string json = "{" +
            //    "\"name\":\"AVE\"," +
            //    "\"stds\":" +
            //        "[" +
            //            "{\"nr\":27721,\"name\":\"Ze Manel\"}," +
            //            "{\"nr\":15642,\"name\":\"Maria Papoila\"}," +
            //            "null," +
            //            "null" +
            //        "]" +
            //    "}";
            Course actual = JsonConvert.DeserializeObject<Course>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJsonfierArrayStrings()
        {
            string[] expected = { "ISEL", "AVE", "SV" };
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(expected, Jsonfier.Selection.Fields);

            string[] actual = JsonConvert.DeserializeObject<string[]>(json);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJSonfierNullValue()
        {
            object src = null;
            string actual = Jsonfier.ToJson(src);
            Assert.AreEqual("null", actual);
        }

    }
}
