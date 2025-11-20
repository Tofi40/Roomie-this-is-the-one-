using Microsoft.AspNetCore.Mvc;
using RoomieSystem.Model.Entities;
using RoomieSystem.Model.Repositories;

namespace RoomieSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly MatchRepository _repository;

    public MatchController(MatchRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IEnumerable<Match> GetAll()
    {
        return _repository.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Match> GetById(int id)
    {
        var match = _repository.GetById(id);
        if (match == null) return NotFound();
        return Ok(match);
    }

    [HttpGet("user/{userId}")]
    public IEnumerable<Match> GetByUser(int userId)
    {
        return _repository.GetByUserId(userId);
    }

    [HttpGet("room/{roomId}")]
    public IEnumerable<Match> GetByRoom(int roomId)
    {
        return _repository.GetByRoomId(roomId);
    }

    [HttpPost]
    public ActionResult Create(Match match)
    {
        var success = _repository.Insert(match);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not create match");
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete match");
        }

        return NoContent();
    }
}
