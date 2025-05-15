namespace Todo.Contracts
{
    public class CreateUserDto
    {
        public string Username { get; set; }

        [System.ComponentModel.DataAnnotations.EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}