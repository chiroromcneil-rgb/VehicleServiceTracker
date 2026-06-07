using System;

namespace VehicleServiceTracker.Models
{
    public abstract class Vehicle
    {
        private string registrationNumber;
        private string make;
        private string model;
        private int year;

        public int VehicleId { get; set; }
        public Owner Owner { get; set; }

        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Registration number is required.");
                registrationNumber = value.Trim().ToUpper();
            }
        }

        public string Make
        {
            get { return make; }
            set { make = string.IsNullOrWhiteSpace(value) ? "Unknown" : value.Trim(); }
        }

        public string Model
        {
            get { return model; }
            set { model = string.IsNullOrWhiteSpace(value) ? "Unknown" : value.Trim(); }
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1950 || value > DateTime.Now.Year + 1)
                    throw new ArgumentException("Vehicle year is not valid.");
                year = value;
            }
        }

        protected Vehicle() { }

        protected Vehicle(int vehicleId, string registrationNumber, string make, string model, int year, Owner owner)
        {
            VehicleId = vehicleId;
            RegistrationNumber = registrationNumber;
            Make = make;
            Model = model;
            Year = year;
            Owner = owner;
        }

        public abstract string VehicleType { get; }

        public virtual string GetDisplayName()
        {
            return RegistrationNumber + " - " + Year + " " + Make + " " + Model;
        }
    }
}
