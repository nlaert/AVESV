using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapperPrj;
using System.Collections.Generic;
using AutoMapperTest.Model;

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

        [TestMethod]
        public void MapToArrayTest()
        {
            Student[] stds = { new Student { Nr = 27721, Name = "Ze Manel" }, new Student { Nr = 15642, Name = "Maria Papoila" } };
            Person[] expected = { new Person { Nr = 27721, Name = "Ze Manel" }, new Person { Nr = 15642, Name = "Maria Papoila" } };
            Mapper<Student, Person> m = Builder.Build<Student, Person>().CreateMapper();
            Person[] actual = m.MapToArray(stds);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MapLazyTest()
        {
            Student[] stds = { new Student { Nr = 27721, Name = "Ze Manel" }, new Student { Nr = 15642, Name = "Maria Papoila" } };
            List<Person> expected = new List<Person>();
            expected.Add(new Person { Nr = 27721, Name = "Ze Manel" });
            expected.Add(new Person { Nr = 15642, Name = "Maria Papoila" });
            Mapper<Student, Person> m = Builder.Build<Student, Person>().CreateMapper();
            IEnumerable<Person> actual = m.MapLazy(stds);
            IEnumerator<Person> exp = expected.GetEnumerator();
            IEnumerator<Person> act = actual.GetEnumerator();

            while (exp.MoveNext() && act.MoveNext())
            {
                Assert.AreEqual(exp.Current, act.Current);
            }
        }

        [TestMethod]
        public void IgnoreMemberTest()
        {
            Mapper<Student, Person> m = Builder.Build<Student, Person>().IgnoreMember("Name").CreateMapper();
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = m.Map(s);
            Assert.AreNotEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Nr);
        }

        [TestMethod]
        public void IgnoreAttributeTest()
        {
            Mapper<Professor, Person> m = Builder.Build<Professor, Person>().IgnoreMember<MapperIgnoreAttribute>().CreateMapper();
            Professor s = new Professor { Nr = 27721, Name = "Ze Manel" };
            Person p = m.Map(s);
            Assert.AreNotEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Nr);
        }

        [TestMethod]
        public void ForMemberTest()
        {
            Mapper<Student, User> m = Builder.Build<Student, User>().ForMember("Id", src => src.Nr.ToString()).CreateMapper();
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            User p = m.Map(s);
            Assert.AreNotEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr.ToString(), p.Id);
        }
    }
}
