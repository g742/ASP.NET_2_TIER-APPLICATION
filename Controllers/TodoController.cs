using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        // Constructor to inject the DbContext
        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // GET: Todo
        public async Task<IActionResult> Index()
        {
            // Retrieve the list of TodoItems from the database
            var todoList = await _context.TodoItems.ToListAsync();
            return View(todoList);
        }

        // POST: Todo/Add
        [HttpPost]
        public async Task<IActionResult> Add(string task)
        {
            if (!string.IsNullOrEmpty(task))
            {
                var newTodo = new TodoItem
                {
                    Task = task,
                    IsCompleted = false
                };

                // Add new TodoItem to the database
                _context.TodoItems.Add(newTodo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // POST: Todo/Complete
        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo != null)
            {
                todo.IsCompleted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // POST: Todo/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo != null)
            {
                _context.TodoItems.Remove(todo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
