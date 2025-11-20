namespace RoomieSystem.Model.Entities;

public class Match
{
    public Match(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public int User1Id { get; set; }

    public int User2Id { get; set; }

    public int RoomId { get; set; }

    public DateTime MatchedAt { get; set; }
}
