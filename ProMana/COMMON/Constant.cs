using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON
{
    public static class ProjectStatusKey
    {
        public const int Opened = 1;
        public const int Closed = 10;
    }

    public static class ProjectStatus
    {
        public const string Opened = "Opened";
        public const string Closed = "Closed";
    }

    public static class TaskStatusKey
    {
        public const int Opened = 1;
        public const int Closed = 10;
        public const int InProgress = 2;
        public const int Resolved = 3;
        public const int ReOpened = 4;
    }

    public static class TaskStatus
    {
        public const string Opened = "Opened";
        public const string Closed = "Closed";
        public const string InProgress = "In Progress";
        public const string Resolved = "Resolved";
        public const string ReOpened = "ReOpened";
    }

    public static class RequestStatusKey
    {
        public const int Draft = 5;
        public const int PendingApproval = 6;
        public const int Approved = 7;
        public const int Rejected = 8;
        public const int Cancelled = 9;
    }

    public static class RequestStatus
    {
        public const string Draft = "Draft";
        public const string PendingApproval = "Pending Approval";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string Cancelled = "Cancelled";
    }

    public static class HardFixJobRole
    {
        public const int PM = 1;
        public const int TeamLead = 2;
        public const int Watcher = 3;
    }

    public static class HardFixJobRoleTitle
    {
        public const string PM = "PM";
        public const string TeamLead = "Team Lead";
        public const string Watcher = "Watcher";
    }

    public static class StoreProcedure
    {
        public const string GetUserDoNotInProject = "dbo.GetUserDoNotInProject";
        public const string GetMyProject = "dbo.GetMyproject";
    }

    public static class CONSTANCT
    {
        public static Dictionary<string,string> Severities = new Dictionary<string, string> { { "1.Normal","The severity of the bug has not been assessed or not yet classified" },
                                                                                              { "2.Text/Cosmetic" ,"Error related to interface, content. The degree of seriousness is assessed on cosmetic regulations"},
                                                                                              { "3.Minor" ,"The bugs do not damage the system's usability. Bug may be due to user rating or tester. There are cases that the proposal is not in the scope of the project"},
                                                                                              { "4.Major","These bugs affect the functionality of the system and the system still works but give false, incomplete, inaccurate results, affecting other features." },
                                                                                              { "5.Crash/Critial" ,"Extremely serious bugs, the system doesn't work. The user cannot perform the next operation in the system"} };
        public static Dictionary<string, string> Priorities = new Dictionary<string,string> { { "1.N/A" ,"These are issues that have not identified priorities"},
                                                                                              { "2.Low" ,"There are issues with low priority, usually without affecting product release. Can be processed later"},
                                                                                              { "3.Normal","These are issues with normal money. With these issues, the assigner handles the proposed plan" },
                                                                                              { "4.High","There are high priority issues. With the issue of these issues, people who are assigned a handle should do it as soon as possible" },
                                                                                              { "5.Urgent","These are issues that have urgent priority, need urgent handling. When the issue has this priority, it should be handled during the session or at least 24 hours" } };
        public static IEnumerable<Filter> TaskListFilter = new List<Filter> { new Filter { Title = "All", FilterType = "All"},
                                                                              new Filter {Title = "Created By Me",FilterType="ByCreaatedBy" },
                                                                              new Filter {Title = "Assigned To Me",FilterType="ByAssignedTo" }};

        public static IEnumerable<Filter> RequestListFilter = new List<Filter> { new Filter { Title = "All", FilterType = "All"},
                                                                                  new Filter {Title = "Created By Me",FilterType="ByCreaatedBy" },
                                                                                  new Filter {Title = "Reported To Me",FilterType="ByAssignedTo" }};

        public static IEnumerable<Filter> TaskStatusFilter = new List<Filter> { new Filter { Title = "All", FilterType = "0"},
                                                                                new Filter {Title = "Opened",FilterType=TaskStatusKey.Opened.ToString() },
                                                                                new Filter {Title = "In Progress",FilterType=TaskStatusKey.InProgress.ToString() },
                                                                                new Filter {Title = "Resolved",FilterType=TaskStatusKey.Resolved.ToString() },
                                                                                new Filter {Title = "Closed",FilterType=TaskStatusKey.Closed.ToString() }};

        public static IEnumerable<Filter> RequestStatusFilter = new List<Filter> { new Filter { Title = "All", FilterType = "0"},
                                                                                new Filter {Title = "Pending Approve",FilterType=RequestStatusKey.PendingApproval.ToString() },
                                                                                new Filter {Title = "Approved",FilterType=RequestStatusKey.Approved.ToString() },
                                                                                new Filter {Title = "Rejected",FilterType=RequestStatusKey.Rejected.ToString()},
                                                                                new Filter {Title = "Cancelled",FilterType=RequestStatusKey.Cancelled.ToString()}};
    }

    public class Filter
    {
        public string Title { get; set; }
        public string FilterType { get; set; }
    }
}
