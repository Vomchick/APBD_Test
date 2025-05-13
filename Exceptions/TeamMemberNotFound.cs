namespace Test1.Exceptions
{
    public class TeamMemberNotFound : Exception
    {
        public TeamMemberNotFound(string message) : base(message)
        {
        }
    }
}
