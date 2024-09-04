using Solid.Attendance.Mcs.Models;
using Solid.Core.Services.Implementation;
using Solid.Core.Services.Repository;

namespace Solid.Attendance.Mcs.Services
{

    public class ScheduledServices : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IDMLServices _dmlServices;
        private readonly IConfiguration _config;

        public ScheduledServices(IConfiguration config)
        {
            _config = config;
            _dmlServices = new DMLServices(_config.GetConnectionString("DefaultConnection"));
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Calculate the time until the next 00:01 AM
            DateTime now = DateTime.Now;
            DateTime nextRunTime = now.Date.AddDays(1).AddMinutes(1); // 00:01 AM of the next day
            TimeSpan initialDelay = nextRunTime - now;

            _timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromDays(1)); // Runs daily at 00:01 AM
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                // Execute the stored procedure for the previous day's date
                await _dmlServices.ExecuteStoredProcedureNonQueryAsync("UpdateOutTimeScheduler", new UpdateOutTimeScheduler { PunchInDate = DateTime.Today });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }


        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
