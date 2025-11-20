namespace RoomieSystem.Model.Entities;

public class UserLike
{
    public UserLike(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public int FromUserId { get; set; }

    public int ToUserId { get; set; }

    public DateTime CreatedAt { get; set; }
}
