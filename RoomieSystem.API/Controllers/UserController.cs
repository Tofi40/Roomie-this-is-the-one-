using Microsoft.AspNetCore.Mvc;
using RoomieSystem.Model.Entities;
using RoomieSystem.Model.Repositories;

namespace RoomieSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserRepository _repository;

    public UserController(UserRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IEnumerable<User> GetAll()
    {
        return _repository.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetById(int id)
    {
        var user = _repository.GetById(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public ActionResult Create(User user)
    {
        var success = _repository.Insert(user);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not create user");
        }

        return Ok();
    }

    [HttpPut]
    public ActionResult Update(User user)
    {
        var existing = _repository.GetById(user.Id);
        if (existing == null)
        {
            return NotFound();
        }

        var success = _repository.Update(user);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existing = _repository.GetById(id);
        if (existing == null)
        {
            return NotFound();
        }

        var success = _repository.Delete(id);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete user");
        }

        return NoContent();
    }
}
