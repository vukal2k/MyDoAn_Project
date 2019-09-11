using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProjectStatistical
    {
        public Project Project { get; set; }
        public IEnumerable<MemberPoint> MemberPoints { get; set; }
        public IEnumerable<TaskStatisticByModule> TaskStatisticByModules { get; set; }
        public IEnumerable<RequestStatisticByModule> RequestStatisticByModules { get; set; }
        public IEnumerable<TaskByTaskType> TaskByTaskTypes { get; set; }
        public IEnumerable<ProjectLog> ProjectLogs { get; set; }
    }

    public class MemberPoint
    {
        public UserInfo UserInfo { get; set; }
        public MemberTaskByStatus TotalCreatedTask { get; set; }
        public MemberTaskByStatus TotalAssignTask { get; set; }
        public MemberRequestByStatus TotalCreatedRequest { get; set; }
        public MemberRequestByStatus TotalAssignRequest { get; set; }
    }

    public class MemberTaskByStatus
    {
        public int Total { get; set; }
        public int TotalByOpen { get; set; }
        public int TotalByInProgress { get; set; }
        public int TotalByResolve { get; set; }
        public int TotalByClosed { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }

    public class MemberRequestByStatus
    {
        public int Total { get; set; }
        public int TotalPendingApproved { get; set; }
        public int TotalApproved { get; set; }
        public int TotalRejected { get; set; }
        public int TotalCancelled { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }

    public class TaskStatisticByModule
    {
        public Module Module { get; set; }
        public TaskByStatus TaskByOpen { get; set; }
        public TaskByStatus TaskByInProgress { get; set; }
        public TaskByStatus TaskByResolve { get; set; }
        public TaskByStatus TaskByClosed { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }

    public class TaskByStatus
    {
        public float Percent { get; set; }
        public int Total { get; set; }

        public IEnumerable<Task> Tasks { get; set; }
    }

    public class RequestStatisticByModule
    {
        public Module Module { get; set; }
        public RequestByStatus RequestByPendingApproved { get; set; }
        public RequestByStatus RequestByApproved { get; set; }
        public RequestByStatus RequestByRejected { get; set; }
        public RequestByStatus RequestByCancelled { get; set; }
    }

    public class RequestByStatus
    {
        public float Percent { get; set; }
        public int Total { get; set; }

        public IEnumerable<Task> Tasks { get; set; }
    }

    public class TaskByTaskType
    {
        public string TaskType { get; set; }
        public int Total { get; set; }
        public float Percent { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}
