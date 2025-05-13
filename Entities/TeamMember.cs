namespace Test1.Entities
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<Task> TasksAssigned { get; set; }
        public IEnumerable<Task> TasksCreated { get; set; }
    }
}
