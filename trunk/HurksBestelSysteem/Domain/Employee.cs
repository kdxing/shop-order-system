namespace HurksBestelSysteem.Domain
{
    public sealed class Employee
    {
        public int internalID;
        public string name;

        public Employee(string name)
        {
            this.name = name;
            this.internalID = -1;
        }

        public Employee(string name, int internalID)
        {
            this.name = name;
            this.internalID = internalID;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
