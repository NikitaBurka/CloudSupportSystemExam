using System.ComponentModel.DataAnnotations;

namespace CloudSupportSystem.Models
{
    public enum Priority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum Status
    {
        Open,
        InProgress,
        Resolved,
        Closed
    }

    public class SupportRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть ім'я користувача")]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть тему заявки")]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть опис проблеми")]
        public string Description { get; set; } = string.Empty;

        public Priority Priority { get; set; } = Priority.Medium;

        public Status Status { get; set; } = Status.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}