using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourses.Models
{
    public class InfoBlock
    {
        public int ID { get; set; }
        public Lesson Lesson { get; set; }
        public int Order { get; set; }
    }
}
