using Microsoft.AspNetCore.Mvc;
using RoomieSystem.Model.Entities;
using RoomieSystem.Model.Repositories;

namespace RoomieSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomSwipeController : ControllerBase
{
    private readonly RoomSwipeRepository _repository;

    public RoomSwipeController(RoomSwipeRepository repository)
    {
        _repository = repository;
    }

    // GET: api/RoomSwipe
    [HttpGet]
    public IEnumerable<RoomSwipe> GetAll()
    {
        return _repository.GetAll();
    }

    // GET: api/RoomSwipe/5
    [HttpGet("{id}")]
    public ActionResult<RoomSwipe> GetById(int id)
    {
        var swipe = _repository.GetById(id);
        if (swipe == null)
        {
            return NotFound();
        }

        return Ok(swipe);
    }

    // GET: api/RoomSwipe/user/3
    [HttpGet("user/{userId}")]
    public IEnumerable<RoomSwipe> GetByUser(int userId)
    {
        return _repository.GetByUserId(userId);
    }

    // GET: api/RoomSwipe/room/2
    [HttpGet("room/{roomId}")]
    public IEnumerable<RoomSwipe> GetByRoom(int roomId)
    {
        return _repository.GetByRoomId(roomId);
    }

    // POST: api/RoomSwipe
    // Body: { "id": 0, "userId": 6, "roomId": 1, "isLiked": true }
    [HttpPost]
    public ActionResult Create(RoomSwipe swipe)
    {
        var success = _repository.Insert(swipe);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not save swipe");
        }

        return Ok();
    }

    // DELETE: api/RoomSwipe/5
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete swipe");
        }

        return NoContent();
    }
}
