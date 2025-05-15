using Todo.AppDbContext;
using Todo.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Todo.Repository.Implementations;



 public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Todo>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Domain.Todo?> GetByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<Domain.Todo> CreateAsync(Domain.Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Domain.Todo> UpdateAsync(Domain.Todo todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                return false;

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }
    }