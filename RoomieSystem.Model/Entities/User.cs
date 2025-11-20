namespace RoomieSystem.Model.Entities;

public class User
{
    public User(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Bio { get; set; }

    public DateTime BirthDate { get; set; }

    public string Gender { get; set; }

    public bool IsSmoker { get; set; }

    public bool HasPets { get; set; }

    public DateTime CreatedAt { get; set; }
}
