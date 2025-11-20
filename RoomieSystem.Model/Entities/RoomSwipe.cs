namespace RoomieSystem.Model.Entities;

public class RoomSwipe
{
    public RoomSwipe(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoomId { get; set; }

    public bool IsLiked { get; set; }

    public DateTime CreatedAt { get; set; }
}
