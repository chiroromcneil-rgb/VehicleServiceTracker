using System;

namespace VehicleServiceTracker.Models
{
    public class MaintenanceRecord
    {
        private decimal cost;

        public int RecordId { get; set; }
        public int VehicleId { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; }

        public decimal Cost
        {
            get { return cost; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Cost cannot be negative.");
                cost = value;
            }
        }

        public MaintenanceRecord() { }

        public MaintenanceRecord(int recordId, int vehicleId, DateTime serviceDate, string description, decimal cost)
        {
            RecordId = recordId;
            VehicleId = vehicleId;
            ServiceDate = serviceDate;
            Description = description;
            Cost = cost;
        }
    }
}
