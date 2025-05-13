namespace Test1.Contracts.Responses
{
    public class TeamMemberResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<TaskResponse> TasksAssigned { get; set; }
        public ICollection<TaskResponse> TasksCreated { get; set; }
    }
}
