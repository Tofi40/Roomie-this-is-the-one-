using RoomieSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace RoomieSystem.Model.Repositories;

public class UserRepository : BaseRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration) { }

    public List<User> GetAll()
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<User>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       first_name,
                       last_name,
                       email,
                       phone,
                       bio,
                       birth_date,
                       gender,
                       is_smoker,
                       has_pets,
                       created_at
                from users
                order by created_at desc";

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new User(Convert.ToInt32(data["id"]))
                    {
                        FirstName  = data["first_name"].ToString(),
                        LastName   = data["last_name"].ToString(),
                        Email      = data["email"].ToString(),
                        Phone      = data["phone"].ToString(),
                        Bio        = data["bio"].ToString(),
                        BirthDate  = Convert.ToDateTime(data["birth_date"]),
                        Gender     = data["gender"].ToString(),
                        IsSmoker   = Convert.ToBoolean(data["is_smoker"]),
                        HasPets    = Convert.ToBoolean(data["has_pets"]),
                        CreatedAt  = Convert.ToDateTime(data["created_at"])
                    });
                }
            }

            return list;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public User? GetById(int id)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       first_name,
                       last_name,
                       email,
                       phone,
                       bio,
                       birth_date,
                       gender,
                       is_smoker,
                       has_pets,
                       created_at
                from users
                where id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new User(Convert.ToInt32(data["id"]))
                {
                    FirstName  = data["first_name"].ToString(),
                    LastName   = data["last_name"].ToString(),
                    Email      = data["email"].ToString(),
                    Phone      = data["phone"].ToString(),
                    Bio        = data["bio"].ToString(),
                    BirthDate  = Convert.ToDateTime(data["birth_date"]),
                    Gender     = data["gender"].ToString(),
                    IsSmoker   = Convert.ToBoolean(data["is_smoker"]),
                    HasPets    = Convert.ToBoolean(data["has_pets"]),
                    CreatedAt  = Convert.ToDateTime(data["created_at"])
                };
            }

            return null;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool Insert(User u)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                insert into users
                    (first_name,
                     last_name,
                     email,
                     phone,
                     bio,
                     birth_date,
                     gender,
                     is_smoker,
                     has_pets)
                values
                    (@first_name,
                     @last_name,
                     @email,
                     @phone,
                     @bio,
                     @birth_date,
                     @gender,
                     @is_smoker,
                     @has_pets)";

            cmd.Parameters.AddWithValue("@first_name", NpgsqlDbType.Text,   u.FirstName);
            cmd.Parameters.AddWithValue("@last_name",  NpgsqlDbType.Text,   u.LastName);
            cmd.Parameters.AddWithValue("@email",      NpgsqlDbType.Text,   u.Email);
            cmd.Parameters.AddWithValue("@phone",      NpgsqlDbType.Text,   (object?)u.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@bio",        NpgsqlDbType.Text,   u.Bio);
            cmd.Parameters.AddWithValue("@birth_date", NpgsqlDbType.Date,   u.BirthDate);
            cmd.Parameters.AddWithValue("@gender",     NpgsqlDbType.Text,   u.Gender);
            cmd.Parameters.AddWithValue("@is_smoker",  NpgsqlDbType.Boolean,u.IsSmoker);
            cmd.Parameters.AddWithValue("@has_pets",   NpgsqlDbType.Boolean,u.HasPets);

            return InsertData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool Update(User u)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                update users
                set first_name = @first_name,
                    last_name  = @last_name,
                    email      = @email,
                    phone      = @phone,
                    bio        = @bio,
                    birth_date = @birth_date,
                    gender     = @gender,
                    is_smoker  = @is_smoker,
                    has_pets   = @has_pets
                where id = @id";

            cmd.Parameters.AddWithValue("@first_name", NpgsqlDbType.Text,   u.FirstName);
            cmd.Parameters.AddWithValue("@last_name",  NpgsqlDbType.Text,   u.LastName);
            cmd.Parameters.AddWithValue("@email",      NpgsqlDbType.Text,   u.Email);
            cmd.Parameters.AddWithValue("@phone",      NpgsqlDbType.Text,   (object?)u.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@bio",        NpgsqlDbType.Text,   u.Bio);
            cmd.Parameters.AddWithValue("@birth_date", NpgsqlDbType.Date,   u.BirthDate);
            cmd.Parameters.AddWithValue("@gender",     NpgsqlDbType.Text,   u.Gender);
            cmd.Parameters.AddWithValue("@is_smoker",  NpgsqlDbType.Boolean,u.IsSmoker);
            cmd.Parameters.AddWithValue("@has_pets",   NpgsqlDbType.Boolean,u.HasPets);
            cmd.Parameters.AddWithValue("@id",         NpgsqlDbType.Integer,u.Id);

            return UpdateData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool Delete(int id)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "delete from users where id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }
}
