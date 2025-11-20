namespace RoomieSystem.Model.Entities;

public class Room
{
    public Room(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public int OwnerUserId { get; set; }

    public string Title { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public int Price { get; set; }

    public int SquareMeters { get; set; }

    public string Description { get; set; }

    public DateTime AvailabilityDate { get; set; }

    public DateTime CreatedAt { get; set; }
}
