namespace Todo.Contracts.Interfaces;


public interface ITodoService
    {
        Task<IEnumerable<Domain.Todo>> GetAllTodosAsync(int userId);
        Task<Domain.Todo> GetTodoByIdAsync(int id, int userId);
        Task<Domain.Todo> CreateTodoAsync(CreateTodoDto createTodoDto, int userId);
        Task<Domain.Todo> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto, int userId);
        Task<bool> DeleteTodoAsync(int id, int userId);
    }