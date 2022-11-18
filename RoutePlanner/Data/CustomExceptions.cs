namespace RoutePlanner.Data
{
    public class UserIsNullException : Exception
    {
        public UserIsNullException() { }
        public UserIsNullException(string message) : base(message) { }
        public UserIsNullException(string message, Exception inner) : base(message, inner) { }
    }

    public class NoDataInDatabaseException : Exception
    {
        public NoDataInDatabaseException() { }
        public NoDataInDatabaseException(string message) : base(message) { }
        public NoDataInDatabaseException(string message, Exception inner) : base(message, inner) { }
    }

    public class DatabaseException : Exception
    {
        public DatabaseException() { }
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception inner) : base(message, inner) { }
    }

    public class ValueNotFoundInConfigurationException : Exception
    {
        public ValueNotFoundInConfigurationException() { }
        public ValueNotFoundInConfigurationException(string message) : base(message) { }
        public ValueNotFoundInConfigurationException(string message, Exception inner) : base(message, inner) { }

    }
}
