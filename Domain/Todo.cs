

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Domain;

public class Todo
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        
        // Foreign key
        public int UserId { get; set; }
        
        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }
    }