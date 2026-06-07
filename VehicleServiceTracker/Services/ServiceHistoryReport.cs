using System.Linq;
using System.Text;
using VehicleServiceTracker.Models;

namespace VehicleServiceTracker.Services
{
    public class ServiceHistoryReport : IServiceReport
    {
        private readonly ServiceManager manager;

        public ServiceHistoryReport(ServiceManager manager)
        {
            this.manager = manager;
        }

        public string GenerateReport(int vehicleId)
        {
            Vehicle vehicle = manager.GetVehicleById(vehicleId);
            if (vehicle == null) return "Vehicle not found.";

            StringBuilder report = new StringBuilder();
            report.AppendLine("SERVICE HISTORY REPORT");
            report.AppendLine(vehicle.GetDisplayName());
            report.AppendLine("Owner: " + vehicle.Owner.Name);
            report.AppendLine("--------------------------------");

            var records = manager.MaintenanceRecords.Where(r => r.VehicleId == vehicleId).OrderByDescending(r => r.ServiceDate).ToList();
            if (records.Count == 0)
            {
                report.AppendLine("No maintenance history found.");
            }
            else
            {
                foreach (var record in records)
                {
                    report.AppendLine(record.ServiceDate.ToShortDateString() + " - " + record.Description + " - $" + record.Cost.ToString("0.00"));
                }
            }

            return report.ToString();
        }
    }
}
