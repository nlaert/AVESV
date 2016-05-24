using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapperPrj;
using System.Collections.Generic;

namespace AutoMapperTest
{
    [TestClass]
    public class AutoMapperTest
    {
        [TestMethod]
        public void MapTest()
        {
            Mapper<Student, Person> m = Builder.Build<Student, Person>().CreateMapper();
            Student s = new Student{ Nr = 27721, Name = "Ze Manel" }; 
            Person p = m.Map(s); 
            Assert.AreEqual(s.Name, p.Name); 
            Assert.AreEqual(s.Nr, p.Nr);
        }
        [TestMethod]
        public void MapCollectionTest()
        {
            Student[] stds = { new Student { Nr = 27721, Name = "Ze Manel" }, new Student { Nr = 15642, Name = "Maria Papoila" } }; 
            Person[] expected = { new Person { Nr = 27721, Name = "Ze Manel" }, new Person { Nr = 15642, Name = "Maria Papoila" } }; 
            Mapper<Student, Person> m = Builder.Build<Student, Person>().CreateMapper();
            List<Person> ps = m.Map<List<Person>>(stds);
            Person[] actual = ps.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
