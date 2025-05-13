namespace Test1.Exceptions
{
    public class TaskTypeNotFoundException : Exception
    {
        public TaskTypeNotFoundException(string message) : base(message)
        {
        }
    }
}
