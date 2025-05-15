


using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Todo.Contracts;
using Todo.Contracts.Interfaces;
using Todo.Repository.Interfaces;

namespace Todo.Application;




public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<Domain.Todo>> GetAllTodosAsync(int userId)
        {
            var todos = await _todoRepository.GetAllByUserIdAsync(userId);
            return todos;
        }

        public async Task<Domain.Todo> GetTodoByIdAsync(int id, int userId)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            
            if (todo == null || todo.UserId != userId)
                throw new KeyNotFoundException("Todo not found or does not belong to the user.");
                
            return todo;
        }

        public async Task<Domain.Todo> CreateTodoAsync(CreateTodoDto createTodoDto, int userId)
        {
            var todo = new Domain.Todo
            {
                Title = createTodoDto.Title,
                Description = createTodoDto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            var createdTodo = await _todoRepository.CreateAsync(todo);
            return createdTodo;
        }

        public async Task<Domain.Todo> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto, int userId)
        {
            var existingTodo = await _todoRepository.GetByIdAsync(id);
            
            if (existingTodo == null || existingTodo.UserId != userId)
               throw new KeyNotFoundException("Todo not found or does not belong to the user.");

            existingTodo.Title = updateTodoDto.Title;
            existingTodo.Description = updateTodoDto.Description;
            existingTodo.UpdatedAt = DateTime.UtcNow;
            
            // If we're marking as completed and it wasn't completed before
            if (updateTodoDto.IsCompleted && !existingTodo.IsCompleted)
            {
                existingTodo.IsCompleted = true;
                existingTodo.CompletedAt = DateTime.UtcNow;
            }
            // If we're marking as not completed and it was completed before
            else if (!updateTodoDto.IsCompleted && existingTodo.IsCompleted)
            {
                existingTodo.IsCompleted = false;
                existingTodo.CompletedAt = null;
            }

            var updatedTodo = await _todoRepository.UpdateAsync(existingTodo);
            return updatedTodo;
        }

        public async Task<bool> DeleteTodoAsync(int id, int userId)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            
            if (todo == null || todo.UserId != userId)
                return false;
                
            return await _todoRepository.DeleteAsync(id);
        }

       
    }