using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Jsonzai.Test.Model;
using Jsonzai.Instr;

namespace Jsonzai.Test
{
    [TestClass]
    public class JsoninstrTest
    {
        [TestMethod]
        public void TestJsoninstrStudent()
        {
            Jsonzai.Test.Model.Student expected = new Jsonzai.Test.Model.Student(27721, "Ze Manel");
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsoninstr.ToJson(expected);
            //string json = "{\"Nr\":27721,\"Name\":\"Ze Manel\"}";
            Jsonzai.Test.Model.Student actual = JsonConvert.DeserializeObject<Jsonzai.Test.Model.Student>(json);
            Assert.AreEqual(expected, actual);
        }

       [TestMethod]
        public void TestJsoninstrArrayPrimitives()
        {
            int[] expected = { 4, 5, 6, 7 };
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsoninstr.ToJson(expected);
            //string json = "[4,5,6,7]";
            int[] actual = JsonConvert.DeserializeObject<int[]>(json);
            CollectionAssert.AreEqual(expected, actual);
        }

        //[TestMethod]
        public void TestJsoninstrCourse()
        {
            Course expected = new Course
            (
                "AVE",
                new Jsonzai.Test.Model.Student[4]{
                    new Jsonzai.Test.Model.Student(27721, "Ze Manel"),
                    new Jsonzai.Test.Model.Student(15642, "Maria Papoila"),
                    null,
                    null
                }
            );
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsoninstr.ToJson(expected);
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

        //[TestMethod]
        public void TestJsoninstrArrayStrings()
        {
            string[] expected = { "ISEL", "AVE", "SV" };
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsoninstr.ToJson(expected);

            string[] actual = JsonConvert.DeserializeObject<string[]>(json);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJsoninstrNullValue()
        {
            object src = null;
            string actual = Jsoninstr.ToJson(src);
            Assert.AreEqual("null", actual);
        }

        [TestMethod]
        public void TestJsonfierIntValues()
        {
            int src = 10;
            string actual = Jsoninstr.ToJson(src);
            Assert.AreEqual(src.ToString(), actual);
        }

        [TestMethod]
        public void TestJsonfierDoubleValue()
        {
            double src = 100000.2;
            string actual = Jsoninstr.ToJson(src);
            Assert.AreEqual("100000.2", actual);
        }

        [TestMethod]
        public void TestJsonfierFloatValue()
        {
            float src = 100000.2F;
            string actual = Jsoninstr.ToJson(src);
            Assert.AreEqual("100000.2", actual);
        }
    }
}
