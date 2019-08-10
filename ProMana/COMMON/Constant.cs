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
    }
    

    public static class StoreProcedure
    {
        public const string GetUserDoNotInProject = "dbo.GetUserDoNotInProject";
    }
}
