using System.Linq;
using System.Text;
using VehicleServiceTracker.Models;

namespace VehicleServiceTracker.Services
{
    public class CostSummaryReport : IServiceReport
    {
        private readonly ServiceManager manager;

        public CostSummaryReport(ServiceManager manager)
        {
            this.manager = manager;
        }

        public string GenerateReport(int vehicleId)
        {
            Vehicle vehicle = manager.GetVehicleById(vehicleId);
            if (vehicle == null) return "Vehicle not found.";

            decimal total = manager.CalculateTotalCost(vehicleId);
            int count = manager.MaintenanceRecords.Count(r => r.VehicleId == vehicleId);

            StringBuilder report = new StringBuilder();
            report.AppendLine("COST SUMMARY REPORT");
            report.AppendLine(vehicle.GetDisplayName());
            report.AppendLine("Owner: " + vehicle.Owner.Name);
            report.AppendLine("--------------------------------");
            report.AppendLine("Number of completed services: " + count);
            report.AppendLine("Total maintenance cost: $" + total.ToString("0.00"));
            return report.ToString();
        }
    }
}
