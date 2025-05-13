namespace Test1.Contracts.Responses
{
    public class TaskResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public TaskTypeResponse TaskType { get; set; }
    }
}
