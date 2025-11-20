using Microsoft.AspNetCore.Mvc;
using RoomieSystem.Model.Entities;
using RoomieSystem.Model.Repositories;

namespace RoomieSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    protected RoomRepository Repository { get; }
    public RoomController(RoomRepository repository) { Repository = repository; }

    [HttpGet("{id}")]
    public ActionResult<Room> Get([FromRoute] int id)
    {
        var room = Repository.GetById(id);
        if (room == null) return NotFound();
        return Ok(room);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll()
        => Ok(Repository.GetAll());

    [HttpPost]
    public ActionResult Create([FromBody] Room room)
    {
        if (room == null) return BadRequest("Room info not correct");
        var ok = Repository.Insert(room);
        return ok ? Ok() : BadRequest();
    }

    [HttpPut]
    public ActionResult Update([FromBody] Room room)
    {
        if (room == null) return BadRequest("Room info not correct");
        var existing = Repository.GetById(room.Id);
        if (existing == null) return NotFound($"Room with id {room.Id} not found");
        var ok = Repository.Update(room);
        return ok ? Ok() : BadRequest("Something went wrong");
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        var existing = Repository.GetById(id);
        if (existing == null) return NotFound($"Room with id {id} not found");
        var ok = Repository.Delete(id);
        return ok ? NoContent() : BadRequest($"Unable to delete room with id {id}");
    }
}