using Microsoft.AspNetCore.Mvc;
using RoomieSystem.Model.Entities;
using RoomieSystem.Model.Repositories;

namespace RoomieSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomPhotoController : ControllerBase
{
    private readonly RoomPhotoRepository _repository;

    public RoomPhotoController(RoomPhotoRepository repository)
    {
        _repository = repository;
    }

    // GET: api/RoomPhoto
    [HttpGet]
    public IEnumerable<RoomPhoto> GetAll()
    {
        return _repository.GetAll();
    }

    // GET: api/RoomPhoto/5
    [HttpGet("{id}")]
    public ActionResult<RoomPhoto> GetById(int id)
    {
        var photo = _repository.GetById(id);
        if (photo == null)
        {
            return NotFound();
        }

        return Ok(photo);
    }

    // GET: api/RoomPhoto/room/3   -> all photos for room 3
    [HttpGet("room/{roomId}")]
    public IEnumerable<RoomPhoto> GetByRoomId(int roomId)
    {
        return _repository.GetByRoomId(roomId);
    }

    // POST: api/RoomPhoto
    [HttpPost]
    public ActionResult Create(RoomPhoto photo)
    {
        var success = _repository.Insert(photo);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not create room photo");
        }

        return Ok();
    }

    // PUT: api/RoomPhoto
    [HttpPut]
    public ActionResult Update(RoomPhoto photo)
    {
        var existing = _repository.GetById(photo.Id);
        if (existing == null)
        {
            return NotFound();
        }

        var success = _repository.Update(photo);
        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update room photo");
        }

        return Ok();
    }

    // DELETE: api/RoomPhoto/5
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
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete room photo");
        }

        return NoContent();
    }
}
