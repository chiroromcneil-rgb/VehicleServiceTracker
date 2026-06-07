namespace VehicleServiceTracker.Models
{
    public class Person
    {
        private string name;
        private string contactNumber;

        public string Name
        {
            get { return name; }
            set { name = string.IsNullOrWhiteSpace(value) ? "Unknown" : value.Trim(); }
        }

        public string ContactNumber
        {
            get { return contactNumber; }
            set { contactNumber = string.IsNullOrWhiteSpace(value) ? "N/A" : value.Trim(); }
        }

        public Person() { }

        public Person(string name, string contactNumber)
        {
            Name = name;
            ContactNumber = contactNumber;
        }

        public virtual string GetContactSummary()
        {
            return Name + " (" + ContactNumber + ")";
        }
    }
}
