using System;

namespace lavAspMvclast.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameOfUser { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateClosed { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }

        public int CreaterId { get; set; }
        public int CompliterId { get; set; }

    }
}
