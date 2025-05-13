using System.ComponentModel.DataAnnotations;

namespace Test1.Contracts.Requests
{
    public class TaskRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        [Required]
        public int IdProject { get; set; }
        [Required]
        public int IdTaskType { get; set; }
        [Required]
        public int IdAssignedTo { get; set; }
        [Required]
        public int IdCreator { get; set; }
    }
}
