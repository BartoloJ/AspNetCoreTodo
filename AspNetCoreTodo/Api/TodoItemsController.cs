using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Identity;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AspNetCoreTodo.Api;

[ApiController]
[Authorize]
[Route("api/todoItems")]
public class TodoItemsController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;
    private readonly UserManager<IdentityUser> _userManager;

    public TodoItemsController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
    {
        _todoItemService = todoItemService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return BadRequest();

        var items = await _todoItemService.GetIncompleteItemsAsync(user);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var items = await _todoItemService.GetIncompleteItemsAsync(user);
        var item = items.FirstOrDefault(val => val.Id == id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost("completed/{id}")]
    public async Task<IActionResult> MarkCompleted(Guid id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var result = await _todoItemService.MarkDoneAsync(id, user);
        if (result)
            return Ok();

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] TodoItem value)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        await _todoItemService.AddItemAsync(value, user);
        return CreatedAtAction(nameof(GetById), new { id = value.Id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(Guid id, [FromBody] TodoItem value)
    {
        if (id != value.Id)
            ModelState.AddModelError("Id", "Mismatch");

        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        // todo: implement edit/update for todo item
        return StatusCode((int)HttpStatusCode.NotImplemented);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {

        //  todo: implement _todoItemService.Delete
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();
        var result = await _todoItemService.DeleteAsync(id, user);
        return StatusCode((int)HttpStatusCode.NotImplemented);
    }
}
