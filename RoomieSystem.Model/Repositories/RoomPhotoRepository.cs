using RoomieSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace RoomieSystem.Model.Repositories;

public class RoomPhotoRepository : BaseRepository
{
    public RoomPhotoRepository(IConfiguration configuration) : base(configuration) { }

    public List<RoomPhoto> GetAll()
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<RoomPhoto>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       room_id,
                       url,
                       position,
                       created_at
                from room_photos
                order by room_id, position";

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new RoomPhoto(Convert.ToInt32(data["id"]))
                    {
                        RoomId    = Convert.ToInt32(data["room_id"]),
                        Url       = data["url"].ToString(),
                        Position  = Convert.ToInt32(data["position"]),
                        CreatedAt = Convert.ToDateTime(data["created_at"])
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

    public List<RoomPhoto> GetByRoomId(int roomId)
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<RoomPhoto>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       room_id,
                       url,
                       position,
                       created_at
                from room_photos
                where room_id = @room_id
                order by position";
            cmd.Parameters.Add("@room_id", NpgsqlDbType.Integer).Value = roomId;

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new RoomPhoto(Convert.ToInt32(data["id"]))
                    {
                        RoomId    = Convert.ToInt32(data["room_id"]),
                        Url       = data["url"].ToString(),
                        Position  = Convert.ToInt32(data["position"]),
                        CreatedAt = Convert.ToDateTime(data["created_at"])
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

    public RoomPhoto? GetById(int id)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       room_id,
                       url,
                       position,
                       created_at
                from room_photos
                where id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new RoomPhoto(Convert.ToInt32(data["id"]))
                {
                    RoomId    = Convert.ToInt32(data["room_id"]),
                    Url       = data["url"].ToString(),
                    Position  = Convert.ToInt32(data["position"]),
                    CreatedAt = Convert.ToDateTime(data["created_at"])
                };
            }

            return null;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool Insert(RoomPhoto photo)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                insert into room_photos
                    (room_id,
                     url,
                     position)
                values
                    (@room_id,
                     @url,
                     @position)";

            cmd.Parameters.AddWithValue("@room_id",  NpgsqlDbType.Integer, photo.RoomId);
            cmd.Parameters.AddWithValue("@url",      NpgsqlDbType.Text,    photo.Url);
            cmd.Parameters.AddWithValue("@position", NpgsqlDbType.Integer, photo.Position);

            return InsertData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool Update(RoomPhoto photo)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                update room_photos
                set room_id  = @room_id,
                    url      = @url,
                    position = @position
                where id = @id";

            cmd.Parameters.AddWithValue("@room_id",  NpgsqlDbType.Integer, photo.RoomId);
            cmd.Parameters.AddWithValue("@url",      NpgsqlDbType.Text,    photo.Url);
            cmd.Parameters.AddWithValue("@position", NpgsqlDbType.Integer, photo.Position);
            cmd.Parameters.AddWithValue("@id",       NpgsqlDbType.Integer, photo.Id);

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
            cmd.CommandText = "delete from room_photos where id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }
}
