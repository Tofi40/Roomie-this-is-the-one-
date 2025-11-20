using Microsoft.AspNetCore.Mvc;
using RoomieSystem.Model.Entities;
using RoomieSystem.Model.Repositories;

namespace RoomieSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessageRepository _repository;

    public MessageController(MessageRepository repository)
    {
        _repository = repository;
    }

    // GET: api/Message/match/5
    [HttpGet("match/{matchId}")]
    public IEnumerable<Message> GetByMatch(int matchId)
    {
        return _repository.GetByMatchId(matchId);
    }

    // GET: api/Message/5
    [HttpGet("{id}")]
    public ActionResult<Message> GetById(int id)
    {
        var msg = _repository.GetById(id);
        if (msg == null) return NotFound();
        return Ok(msg);
    }

    // POST: api/Message
    [HttpPost]
    public ActionResult Create(Message message)
    {
        var success = _repository.Insert(message);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not create message");
        }

        return Ok();
    }

    // DELETE: api/Message/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existing = _repository.GetById(id);
        if (existing == null) return NotFound();

        var success = _repository.Delete(id);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete message");
        }

        return NoContent();
    }
}
