using System.ComponentModel.DataAnnotations;

namespace Todo.Domain;


public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation property
        public ICollection<Todo> Todos { get; set; }
    }