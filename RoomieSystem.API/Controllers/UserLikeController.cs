using Microsoft.AspNetCore.Mvc;
using RoomieSystem.Model.Entities;
using RoomieSystem.Model.Repositories;

namespace RoomieSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserLikeController : ControllerBase
{
    private readonly UserLikeRepository _repository;

    public UserLikeController(UserLikeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IEnumerable<UserLike> GetAll()
    {
        return _repository.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<UserLike> GetById(int id)
    {
        var like = _repository.GetById(id);
        if (like == null) return NotFound();
        return Ok(like);
    }

    [HttpGet("from/{userId}")]
    public IEnumerable<UserLike> GetFromUser(int userId)
    {
        return _repository.GetByFromUser(userId);
    }

    [HttpGet("to/{userId}")]
    public IEnumerable<UserLike> GetToUser(int userId)
    {
        return _repository.GetByToUser(userId);
    }

    [HttpGet("mutual")]
    public ActionResult<bool> HasMutualLike([FromQuery] int userAId, [FromQuery] int userBId)
    {
        var hasMutual = _repository.HasMutualLike(userAId, userBId);
        return Ok(hasMutual);
    }

    [HttpPost]
    public ActionResult Create(UserLike like)
    {
        var success = _repository.Insert(like);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not create like");
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existing = _repository.GetById(id);
        if (existing == null) return NotFound();

        var success = _repository.Delete(id);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete like");
        }

        return NoContent();
    }
}
