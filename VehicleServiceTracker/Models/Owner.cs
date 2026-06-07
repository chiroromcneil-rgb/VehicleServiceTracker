namespace VehicleServiceTracker.Models
{
    public class Owner : Person
    {
        public int OwnerId { get; set; }

        public Owner() { }

        public Owner(int ownerId, string name, string contactNumber) : base(name, contactNumber)
        {
            OwnerId = ownerId;
        }

        public override string GetContactSummary()
        {
            return "Owner: " + base.GetContactSummary();
        }
    }
}
