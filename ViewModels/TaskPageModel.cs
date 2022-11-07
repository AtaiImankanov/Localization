using lavAspMvclast.Models;
using System;
using System.Collections.Generic;

namespace lavAspMvclast.ViewModels
{
    public class TaskPageModel
    {
        public IEnumerable<ToDoTask> ToDoTasks { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string Name { get; set; }
        public string WordInDescription { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime Form { get; set; }
        public DateTime To { get; set; }
    }
}
