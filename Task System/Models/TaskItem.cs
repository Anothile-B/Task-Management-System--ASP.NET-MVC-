namespace Task_System.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string? Priority { get; set; }

        public string? AssignedTo { get; set; } // Employee name

        public string? Status { get; set; } = "Pending"; // Pending, In Progress, Completed

        public int Progress { get; set; } = 0; // 0-100%

        public List<string> Comments { get; set; } = new List<string>();
        public bool IsCompleted { get; set; }
    }
}
