﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperTest
{
    public class Professor
    {
        public string Name { get; set; }
        public int Nr { get; set; }
        public string Lesson { get; set; }

        public Professor(string Name, int Nr, string Lesson)
        {
            this.Name = Name;
            this.Nr = Nr;
            this.Lesson = Lesson;
        }
       

    }
}
