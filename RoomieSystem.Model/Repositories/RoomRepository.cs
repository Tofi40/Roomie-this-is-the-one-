using RoomieSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace RoomieSystem.Model.Repositories;

public class RoomRepository : BaseRepository
{
    public RoomRepository(IConfiguration configuration) : base(configuration) { }

    public Room? GetById(int id)
    {
        NpgsqlConnection? dbConn = null;
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       owner_user_id,
                       title,
                       city,
                       address,
                       price,
                       square_meters,
                       description,
                       availability_date,
                       created_at
                from rooms
                where id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new Room(Convert.ToInt32(data["id"]))
                {
                    OwnerUserId     = Convert.ToInt32(data["owner_user_id"]),
                    Title           = data["title"].ToString(),
                    City            = data["city"].ToString(),
                    Address         = data["address"].ToString(),
                    Price           = Convert.ToInt32(data["price"]),
                    SquareMeters    = Convert.ToInt32(data["square_meters"]),
                    Description     = data["description"].ToString(),
                    AvailabilityDate= Convert.ToDateTime(data["availability_date"]),
                    CreatedAt       = Convert.ToDateTime(data["created_at"])
                };
            }
            return null;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public List<Room> GetAll()
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<Room>();
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       owner_user_id,
                       title,
                       city,
                       address,
                       price,
                       square_meters,
                       description,
                       availability_date,
                       created_at
                from rooms
                order by created_at desc";

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new Room(Convert.ToInt32(data["id"]))
                    {
                        OwnerUserId      = Convert.ToInt32(data["owner_user_id"]),
                        Title            = data["title"].ToString(),
                        City             = data["city"].ToString(),
                        Address          = data["address"].ToString(),
                        Price            = Convert.ToInt32(data["price"]),
                        SquareMeters     = Convert.ToInt32(data["square_meters"]),
                        Description      = data["description"].ToString(),
                        AvailabilityDate = Convert.ToDateTime(data["availability_date"]),
                        CreatedAt        = Convert.ToDateTime(data["created_at"])
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

    public bool Insert(Room r)
    {
        NpgsqlConnection? dbConn = null;
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                insert into rooms
                    (owner_user_id,
                     title,
                     city,
                     address,
                     price,
                     square_meters,
                     description,
                     availability_date)
                values
                    (@owner_user_id,
                     @title,
                     @city,
                     @address,
                     @price,
                     @square_meters,
                     @description,
                     @availability_date)";
            cmd.Parameters.AddWithValue("@owner_user_id",  NpgsqlDbType.Integer, r.OwnerUserId);
            cmd.Parameters.AddWithValue("@title",          NpgsqlDbType.Text,    r.Title);
            cmd.Parameters.AddWithValue("@city",           NpgsqlDbType.Text,    r.City);
            cmd.Parameters.AddWithValue("@address",        NpgsqlDbType.Text,    r.Address);
            cmd.Parameters.AddWithValue("@price",          NpgsqlDbType.Integer, r.Price);
            cmd.Parameters.AddWithValue("@square_meters",  NpgsqlDbType.Integer, r.SquareMeters);
            cmd.Parameters.AddWithValue("@description",    NpgsqlDbType.Text,    r.Description);
            cmd.Parameters.AddWithValue("@availability_date", NpgsqlDbType.Date, r.AvailabilityDate);

            return InsertData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool Update(Room r)
    {
        NpgsqlConnection? dbConn = null;
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                update rooms
                set title             = @title,
                    city              = @city,
                    address           = @address,
                    price             = @price,
                    square_meters     = @square_meters,
                    description       = @description,
                    availability_date = @availability_date
                where id = @id";
            cmd.Parameters.AddWithValue("@title",          NpgsqlDbType.Text,    r.Title);
            cmd.Parameters.AddWithValue("@city",           NpgsqlDbType.Text,    r.City);
            cmd.Parameters.AddWithValue("@address",        NpgsqlDbType.Text,    r.Address);
            cmd.Parameters.AddWithValue("@price",          NpgsqlDbType.Integer, r.Price);
            cmd.Parameters.AddWithValue("@square_meters",  NpgsqlDbType.Integer, r.SquareMeters);
            cmd.Parameters.AddWithValue("@description",    NpgsqlDbType.Text,    r.Description);
            cmd.Parameters.AddWithValue("@availability_date", NpgsqlDbType.Date, r.AvailabilityDate);
            cmd.Parameters.AddWithValue("@id",             NpgsqlDbType.Integer, r.Id);

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
            cmd.CommandText = "delete from rooms where id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }
}
