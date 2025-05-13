namespace Test1.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly Deadline { get; set; }
        public Project Project { get; set; }
        public TaksType TaskType { get; set; }
        public TeamMember AssignedTo { get; set; }
        public TeamMember Creator { get; set; }
    }
}
