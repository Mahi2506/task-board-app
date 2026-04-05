using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.API.DTOs;
using TaskBoard.API.Services;

namespace TaskBoard.API.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _tasks;
    public TasksController(ITaskService tasks) => _tasks = tasks;

    private int UserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET /api/tasks
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _tasks.GetAllAsync(UserId));

    // POST /api/tasks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest req)
    {
        try { return Ok(await _tasks.CreateAsync(UserId, req)); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    // PUT /api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest req)
    {
        try { return Ok(await _tasks.UpdateAsync(UserId, id, req)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    // PUT /api/tasks/{id}/complete
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        try { return Ok(await _tasks.CompleteAsync(UserId, id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    // DELETE /api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try { await _tasks.DeleteAsync(UserId, id); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}