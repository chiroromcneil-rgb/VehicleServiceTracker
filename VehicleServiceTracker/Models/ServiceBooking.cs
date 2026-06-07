using System;

namespace VehicleServiceTracker.Models
{
    public class ServiceBooking
    {
        public int BookingId { get; set; }
        public int VehicleId { get; set; }
        public DateTime BookingDate { get; set; }
        public string ServiceType { get; set; }
        public string WorkshopName { get; set; }
        public string Status { get; set; }

        public ServiceBooking() { }

        public ServiceBooking(int bookingId, int vehicleId, DateTime bookingDate, string serviceType, string workshopName, string status)
        {
            BookingId = bookingId;
            VehicleId = vehicleId;
            BookingDate = bookingDate;
            ServiceType = serviceType;
            WorkshopName = workshopName;
            Status = status;
        }

        public bool IsUpcoming()
        {
            return BookingDate.Date >= DateTime.Today && Status != "Completed" && Status != "Cancelled";
        }
    }
}
