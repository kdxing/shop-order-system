namespace HurksBestelSysteem.Domain
{
    public sealed class Customer
    {
        public string firstName;
        public string lastName;
        public string phoneNumber;
        public string street;
        public string streetNumber;
        public string town;
        public int internalID;

        public Customer(string firstName, string lastName, string phoneNumber,
            string street, string streetNumber, string town)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.street = street;
            this.streetNumber = streetNumber;
            this.town = town;
            this.internalID = -1;
        }

        public Customer(string firstName, string lastName, string phoneNumber, 
            string street, string streetNumber, string town, int internalID)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.street = street;
            this.streetNumber = streetNumber;
            this.town = town;
            this.internalID = internalID;
        }

        public override string ToString()
        {
            return lastName + " (" + firstName + ") " + street + " " + streetNumber + ", " + town;
        }
    }
}
