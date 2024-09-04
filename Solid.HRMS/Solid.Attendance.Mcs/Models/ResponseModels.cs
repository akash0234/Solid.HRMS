namespace Solid.Attendance.Mcs.Models
{
    public class ResponseModels
    {
    }
    
    public class AttendanceResponseModel
    {
        public string EmployeeCode { get; set; }
        public string DisplayName { get; set; }
        public DateTime InTime { get; set; }
        public DateTime? OutTime { get; set; } // Nullable to handle cases where OutTime might be null
        public DateTime Date { get; set; }
        public bool IsLate { get; set; } // Nullable to handle cases where IsLate might be null
        public string Notes { get; set; }
    }
}
