using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Contracts;
using Todo.Contracts.Interfaces;

namespace Todo.Controllers;



[ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.Todo>>> GetAllTodos()
        {
            var userId = (int)HttpContext.Items["UserId"];
            var todos = await _todoService.GetAllTodosAsync(userId);
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Domain.Todo>> GetTodoById(int id)
        {
            var userId = (int)HttpContext.Items["UserId"];
            var todo = await _todoService.GetTodoByIdAsync(id, userId);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<Domain.Todo>> CreateTodo([FromBody] CreateTodoDto createTodoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = (int)HttpContext.Items["UserId"];
            var createdTodo = await _todoService.CreateTodoAsync(createTodoDto, userId);

            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Domain.Todo>> UpdateTodo(int id, [FromBody] UpdateTodoDto updateTodoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = (int)HttpContext.Items["UserId"];
            var updatedTodo = await _todoService.UpdateTodoAsync(id, updateTodoDto, userId);

            if (updatedTodo == null)
                return NotFound();

            return Ok(updatedTodo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var userId = (int)HttpContext.Items["UserId"];
            var result = await _todoService.DeleteTodoAsync(id, userId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }