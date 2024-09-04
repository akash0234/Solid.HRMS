using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Solid.Attendance.Mcs.Models;
using Solid.Core.Helpers;
using Solid.Core.Models;
using Solid.Core.Services.Repository;

namespace Solid.Attendance.Mcs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IDMLServices _dmlServices;
        public AttendanceController(IDMLServices dMLServices)
        {
            this._dmlServices = dMLServices;
        }
        [HttpGet]
        [Route("GetAttendance")]
        public async Task<IActionResult> GetAttendance(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var requestModel = new DateRangeRequestModels()
                {
                    StartDate = StartDate,
                    EndDate = EndDate
                };
                var attendance = await _dmlServices.GetDataTable(StoreProcedures.GetEmployeeAttendance, requestModel);

                // Check if the DataTable has any rows
                if (attendance.Rows.Count > 0)
                {
                    var attendanceResponseModelLists = DataTableExtensions.ConvertToList<AttendanceResponseModel>(attendance);
                    // Convert DataTable to JSON or another format if needed
                    //var result = ConvertDataTableToJson(attendance);
                    return Ok(attendanceResponseModelLists); // Return the data with HTTP 200 OK
                }
                else
                {
                    return NotFound("No attendance records found."); // Return HTTP 404 Not Found if no records are found
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }




        }

        [HttpGet]
        [Route("GetEmployeeAttendance")]
        public async Task<IActionResult> GetEmployeeAttendance(int employeID, DateTime startDate, DateTime endDate)
        {
            try
            {
                var requestModel = new EmployeeAttendanceRequestModels()
                {
                    EmployeeID = employeID,
                    StartDate = startDate,
                    EndDate = endDate
                };
                var attendance = await _dmlServices.GetDataTable(StoreProcedures.GetEmployeeAttendanceByID, requestModel);

                // Check if the DataTable has any rows
                if (attendance.Rows.Count > 0)
                {
                    var attendanceResponseModelLists = DataTableExtensions.ConvertToList<AttendanceResponseModel>(attendance);
                    // Convert DataTable to JSON or another format if needed
                    //var result = ConvertDataTableToJson(attendance);
                    return Ok(attendanceResponseModelLists); // Return the data with HTTP 200 OK
                }
                else
                {
                    return NotFound("No attendance records found."); // Return HTTP 404 Not Found if no records are found
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }




        }

       
        [HttpPost]
        [Route("Punch")]
        public async Task<IActionResult> Punch([FromBody] PunchRequestModel punchRequest)
        {
            // Validate the AccessType
            if (punchRequest.AccessType != "In" && punchRequest.AccessType != "Out")
            {
                return BadRequest("Invalid AccessType. Must be 'In' or 'Out'.");
            }

            // Prepare the parameters for inserting into AccessLog
           
            try
            {
                // Insert the record into the AccessLog table And Attendance table if Previous Inconsistant Punch Found.
                await _dmlServices.ExecuteStoredProcedureNonQueryAsync(StoreProcedures.UpdateOutTimeScheduler, new UpdateOutTimeScheduler { PunchInDate = punchRequest.AccessTime });

                // Insert the record into the AccessLog table
                var returnID = await _dmlServices.ExecuteStoredProcedureNonQueryAsync(StoreProcedures.InsertAndUpdateAccessLog, punchRequest, "NewAccessLogID");
                if(returnID > 0)
                {

                    return Ok("Punch recorded successfully.");

                }
                else
                {
                    return BadRequest("Something went wrong, Please try again");
                }
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                // Logger.LogError(ex, "Error recording punch");
                return StatusCode(500, "An error occurred while recording the punch.");
            }
        }

    }
}
