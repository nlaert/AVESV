using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapperPrj;

namespace AutoMapperTest
{
    [TestClass]
    public class AutoMapperTest
    {
        [TestMethod]
        public void MapTest()
        {
            Mapper<Student, Person> m =
            Student s = new Student { Nr = 27721, Name = "Ze Manel" }; 
            Person p = m.Map(s); 
            Assert.AreEqual(s.Name, p.Name); 
            Assert.AreEqual(s.Nr, p.Nr);
        }
    }
}
