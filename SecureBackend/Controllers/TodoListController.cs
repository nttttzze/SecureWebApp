using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SecureWebApp.Entities;
using SecureWebApp.Models;
using SecureWebApp.ViewModels;


namespace SecureWebApp.Controllers;

// [Authorize(Roles = "User, Admin")]
[ApiController]
[Route("api/[controller]")]
public class TodoListController(DataContext context) : ControllerBase
{
    private readonly HtmlSanitizer _htmlSanitizer = new();
    private readonly DataContext _context = context;

    [HttpGet()]
    public async Task<ActionResult> ShowTodoList()
    {
        var list = await _context.Todo
        .Select(list => new
        {
            list.Id,
            list.Task
        }).ToListAsync();
        return Ok(new { success = true, Todo = list });

    }
    [HttpPost("addTask")]
    public async Task<ActionResult> AddTask(TodoListPostViewModel model)
    {
        try
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var todo = new Todo
            {
                Task = _htmlSanitizer.Sanitize(model.Task)
            };

            await _context.Todo.AddAsync(todo);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, data = todo });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }


    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var task = await _context.Todo.FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return NotFound(new { success = false, message = "Task does not exist." });
        }

        _context.Remove(task);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Task removed" });
    }

    [HttpPatch("update/{id}")]
    public async Task<ActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
    {
        var task = await _context.Todo.FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return NotFound(new { success = false, message = "Task does not exist." });
        }

        task.Task = dto.Task;

        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Task updated" });
    }
}
