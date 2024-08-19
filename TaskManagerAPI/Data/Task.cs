using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Data
{
    public class Task
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool Completed { get; set; } = false;
    }
}