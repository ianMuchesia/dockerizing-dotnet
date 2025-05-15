


namespace Todo.Repository.Interfaces;



  public interface ITodoRepository
    {
        Task<IEnumerable<Domain.Todo>> GetAllByUserIdAsync(int userId);
        Task<Domain.Todo?> GetByIdAsync(int id);
        Task<Domain.Todo> CreateAsync(Domain.Todo todo);
        Task<Domain.Todo> UpdateAsync(Domain.Todo todo);
        Task<bool> DeleteAsync(int id);
    }