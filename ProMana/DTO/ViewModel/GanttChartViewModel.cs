using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class GanttChartViewModel
    {
        public Project Project { get; set; }
        public List<DateTime> ListDay { get; set; }
        public int Page { get; set; }
        public List<TaskViewModel> ListTask { get; set; }
    }

    public class TaskViewModel
    {
        public DTO.Task Task { get; set; }
        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
    }
}
