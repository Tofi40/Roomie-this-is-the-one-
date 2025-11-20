namespace RoomieSystem.Model.Entities;

public class RoomPhoto
{
    public RoomPhoto(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public int RoomId { get; set; }

    public string Url { get; set; }

    public int Position { get; set; }

    public DateTime CreatedAt { get; set; }
}
