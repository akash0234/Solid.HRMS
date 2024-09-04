using Solid.Core.Models;
using static Solid.Attendance.Mcs.Models.Enum.AttendanceEnum;

namespace Solid.Attendance.Mcs.Models
{
    public class RequestModels
    {

    }
    
    public class EmployeeAttendanceRequestModels : DateRangeRequestModels
    {
        public int EmployeeID { get; set; }
    }
    public class PunchRequestModel
    {
        public int EmployeeID { get; set; }
        public DateTime AccessTime { get; set; }
        public string AccessType { get; set; }
        public string? Notes { get; internal set; }
    }
    public class UpdateOutTimeScheduler
    {
        public DateTime PunchInDate { get; set; }
    }
    
}
