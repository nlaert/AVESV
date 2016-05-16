using Jsonzai.Instr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Test.Model
{
    public class Course2
    {
        //public Student[] stds;
        public Student std;
        public string name;

        /*
        public Course(string name, Student[] stds)
        {
            this.stds = stds;
            this.name = name;
        }
        */

        public Course2(string name, Student std)
        {
            this.std = std;
            this.name = name;
        }
        /*
        public Student[] Students
        {
            get
            {
                return stds;
            }
            set
            {
                stds = value;
            }
        }
        */

        public Student student
        {
            get
            {
                return std;
            }
            set
            {
                std = value;
            }
        }


        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /*
        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Course other = obj as Course;
            if (other == null) return false;
            return name.Equals(other.name) && Enumerable.SequenceEqual(this.stds, other.stds);
        }
        */
    }

}