using System;

namespace HurksBestelSysteem.Database
{
    [Serializable]
    public class DatabaseException : System.Exception
    {
        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public DatabaseException(string message)
            : base(message)
        {

        }

    }
}
