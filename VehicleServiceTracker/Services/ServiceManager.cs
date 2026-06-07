using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VehicleServiceTracker.Models;

namespace VehicleServiceTracker.Services
{
    public class ServiceManager
    {
        private readonly string dataFolder;
        private readonly string vehiclesFile;
        private readonly string bookingsFile;
        private readonly string recordsFile;

        public List<Vehicle> Vehicles { get; private set; }
        public List<ServiceBooking> Bookings { get; private set; }
        public List<MaintenanceRecord> MaintenanceRecords { get; private set; }

        public ServiceManager()
        {
            dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            vehiclesFile = Path.Combine(dataFolder, "vehicles.csv");
            bookingsFile = Path.Combine(dataFolder, "bookings.csv");
            recordsFile = Path.Combine(dataFolder, "maintenance.csv");

            Vehicles = new List<Vehicle>();
            Bookings = new List<ServiceBooking>();
            MaintenanceRecords = new List<MaintenanceRecord>();
            LoadAll();
        }

        public void AddVehicle(Car car)
        {
            if (Vehicles.Any(v => v.RegistrationNumber.Equals(car.RegistrationNumber, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A vehicle with this registration already exists.");

            car.VehicleId = GetNextVehicleId();
            if (car.Owner == null) car.Owner = new Owner();
            car.Owner.OwnerId = car.VehicleId;
            Vehicles.Add(car);
            SaveVehicles();
        }

        public void UpdateVehicle(Car updatedCar)
        {
            Vehicle existing = GetVehicleById(updatedCar.VehicleId);
            if (existing == null) throw new InvalidOperationException("Vehicle was not found.");

            if (Vehicles.Any(v => v.VehicleId != updatedCar.VehicleId && v.RegistrationNumber.Equals(updatedCar.RegistrationNumber, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Another vehicle already uses this registration.");

            existing.RegistrationNumber = updatedCar.RegistrationNumber;
            existing.Make = updatedCar.Make;
            existing.Model = updatedCar.Model;
            existing.Year = updatedCar.Year;
            existing.Owner = updatedCar.Owner;
            SaveVehicles();
        }

        public void DeleteVehicle(int vehicleId)
        {
            Vehicle vehicle = GetVehicleById(vehicleId);
            if (vehicle == null) throw new InvalidOperationException("Vehicle was not found.");

            Vehicles.Remove(vehicle);
            Bookings.RemoveAll(b => b.VehicleId == vehicleId);
            MaintenanceRecords.RemoveAll(r => r.VehicleId == vehicleId);
            SaveAll();
        }

        public List<Vehicle> SearchVehicles(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return Vehicles.ToList();
            keyword = keyword.Trim().ToLower();
            return Vehicles.Where(v =>
                v.RegistrationNumber.ToLower().Contains(keyword) ||
                v.Make.ToLower().Contains(keyword) ||
                v.Model.ToLower().Contains(keyword) ||
                v.Owner.Name.ToLower().Contains(keyword)).ToList();
        }

        public Vehicle GetVehicleById(int vehicleId)
        {
            return Vehicles.FirstOrDefault(v => v.VehicleId == vehicleId);
        }

        public void AddBooking(ServiceBooking booking)
        {
            if (GetVehicleById(booking.VehicleId) == null) throw new InvalidOperationException("Please select a vehicle.");
            if (string.IsNullOrWhiteSpace(booking.ServiceType)) throw new ArgumentException("Service type is required.");
            if (string.IsNullOrWhiteSpace(booking.WorkshopName)) throw new ArgumentException("Workshop name is required.");

            booking.BookingId = GetNextBookingId();
            Bookings.Add(booking);
            SaveBookings();
        }

        public void UpdateBookingStatus(int bookingId, string status)
        {
            var booking = Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) throw new InvalidOperationException("Booking was not found.");
            booking.Status = status;
            SaveBookings();
        }

        public List<ServiceBooking> GetUpcomingBookings()
        {
            return Bookings.Where(b => b.IsUpcoming()).OrderBy(b => b.BookingDate).ToList();
        }

        public void AddMaintenanceRecord(MaintenanceRecord record)
        {
            if (GetVehicleById(record.VehicleId) == null) throw new InvalidOperationException("Please select a vehicle.");
            if (string.IsNullOrWhiteSpace(record.Description)) throw new ArgumentException("Description is required.");

            record.RecordId = GetNextRecordId();
            MaintenanceRecords.Add(record);
            SaveMaintenanceRecords();
        }

        public decimal CalculateTotalCost(int vehicleId)
        {
            return MaintenanceRecords.Where(r => r.VehicleId == vehicleId).Sum(r => r.Cost);
        }

        public void SaveAll()
        {
            SaveVehicles();
            SaveBookings();
            SaveMaintenanceRecords();
        }

        private void LoadAll()
        {
            try
            {
                Directory.CreateDirectory(dataFolder);
                LoadVehicles();
                LoadBookings();
                LoadMaintenanceRecords();
            }
            catch
            {
                Vehicles = new List<Vehicle>();
                Bookings = new List<ServiceBooking>();
                MaintenanceRecords = new List<MaintenanceRecord>();
            }
        }

        private void LoadVehicles()
        {
            Vehicles.Clear();
            if (!File.Exists(vehiclesFile)) return;

            foreach (string line in File.ReadAllLines(vehiclesFile).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] p = SplitCsv(line);
                if (p.Length < 7) continue;

                int id = int.Parse(p[0]);
                var owner = new Owner(id, p[5], p[6]);
                var car = new Car(id, p[1], p[2], p[3], int.Parse(p[4]), owner);
                Vehicles.Add(car);
            }
        }

        private void LoadBookings()
        {
            Bookings.Clear();
            if (!File.Exists(bookingsFile)) return;

            foreach (string line in File.ReadAllLines(bookingsFile).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] p = SplitCsv(line);
                if (p.Length < 6) continue;
                Bookings.Add(new ServiceBooking(int.Parse(p[0]), int.Parse(p[1]), DateTime.Parse(p[2]), p[3], p[4], p[5]));
            }
        }

        private void LoadMaintenanceRecords()
        {
            MaintenanceRecords.Clear();
            if (!File.Exists(recordsFile)) return;

            foreach (string line in File.ReadAllLines(recordsFile).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] p = SplitCsv(line);
                if (p.Length < 5) continue;
                MaintenanceRecords.Add(new MaintenanceRecord(int.Parse(p[0]), int.Parse(p[1]), DateTime.Parse(p[2]), p[3], decimal.Parse(p[4])));
            }
        }

        private void SaveVehicles()
        {
            Directory.CreateDirectory(dataFolder);
            List<string> lines = new List<string> { "VehicleId,RegistrationNumber,Make,Model,Year,OwnerName,ContactNumber" };
            lines.AddRange(Vehicles.Select(v => string.Join(",", new[]
            {
                v.VehicleId.ToString(), Escape(v.RegistrationNumber), Escape(v.Make), Escape(v.Model), v.Year.ToString(), Escape(v.Owner.Name), Escape(v.Owner.ContactNumber)
            })));
            File.WriteAllLines(vehiclesFile, lines);
        }

        private void SaveBookings()
        {
            Directory.CreateDirectory(dataFolder);
            List<string> lines = new List<string> { "BookingId,VehicleId,BookingDate,ServiceType,WorkshopName,Status" };
            lines.AddRange(Bookings.Select(b => string.Join(",", new[]
            {
                b.BookingId.ToString(), b.VehicleId.ToString(), b.BookingDate.ToString("yyyy-MM-dd"), Escape(b.ServiceType), Escape(b.WorkshopName), Escape(b.Status)
            })));
            File.WriteAllLines(bookingsFile, lines);
        }

        private void SaveMaintenanceRecords()
        {
            Directory.CreateDirectory(dataFolder);
            List<string> lines = new List<string> { "RecordId,VehicleId,ServiceDate,Description,Cost" };
            lines.AddRange(MaintenanceRecords.Select(r => string.Join(",", new[]
            {
                r.RecordId.ToString(), r.VehicleId.ToString(), r.ServiceDate.ToString("yyyy-MM-dd"), Escape(r.Description), r.Cost.ToString()
            })));
            File.WriteAllLines(recordsFile, lines);
        }

        private int GetNextVehicleId() { return Vehicles.Count == 0 ? 1 : Vehicles.Max(v => v.VehicleId) + 1; }
        private int GetNextBookingId() { return Bookings.Count == 0 ? 1 : Bookings.Max(b => b.BookingId) + 1; }
        private int GetNextRecordId() { return MaintenanceRecords.Count == 0 ? 1 : MaintenanceRecords.Max(r => r.RecordId) + 1; }

        private static string Escape(string value)
        {
            if (value == null) return "";
            return value.Replace("\\", "\\\\").Replace(",", "\\,");
        }

        private static string[] SplitCsv(string line)
        {
            List<string> values = new List<string>();
            string current = "";
            bool escape = false;

            foreach (char c in line)
            {
                if (escape)
                {
                    current += c;
                    escape = false;
                }
                else if (c == '\\')
                {
                    escape = true;
                }
                else if (c == ',')
                {
                    values.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }

            values.Add(current);
            return values.ToArray();
        }
    }
}
