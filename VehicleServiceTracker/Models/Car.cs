namespace VehicleServiceTracker.Models
{
    public class Car : Vehicle
    {
        public override string VehicleType { get { return "Car"; } }

        public Car() { }

        public Car(int vehicleId, string registrationNumber, string make, string model, int year, Owner owner)
            : base(vehicleId, registrationNumber, make, model, year, owner)
        {
        }

        public override string GetDisplayName()
        {
            return VehicleType + ": " + base.GetDisplayName();
        }
    }
}
