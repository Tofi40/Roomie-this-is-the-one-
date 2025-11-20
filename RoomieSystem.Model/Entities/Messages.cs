namespace RoomieSystem.Model.Entities;

public class Message
{
    public Message(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public int MatchId { get; set; }

    public int FromUserId { get; set; }

    public string MessageText { get; set; }

    public DateTime SentAt { get; set; }
}
