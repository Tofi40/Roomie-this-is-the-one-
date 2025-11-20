using RoomieSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace RoomieSystem.Model.Repositories;

public class RoomSwipeRepository : BaseRepository
{
    public RoomSwipeRepository(IConfiguration configuration) : base(configuration) { }

    public List<RoomSwipe> GetAll()
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<RoomSwipe>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       user_id,
                       room_id,
                       is_liked,
                       created_at
                from room_swipes
                order by created_at desc";

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new RoomSwipe(Convert.ToInt32(data["id"]))
                    {
                        UserId    = Convert.ToInt32(data["user_id"]),
                        RoomId    = Convert.ToInt32(data["room_id"]),
                        IsLiked   = Convert.ToBoolean(data["is_liked"]),
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

    public RoomSwipe? GetById(int id)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       user_id,
                       room_id,
                       is_liked,
                       created_at
                from room_swipes
                where id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new RoomSwipe(Convert.ToInt32(data["id"]))
                {
                    UserId    = Convert.ToInt32(data["user_id"]),
                    RoomId    = Convert.ToInt32(data["room_id"]),
                    IsLiked   = Convert.ToBoolean(data["is_liked"]),
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

    public List<RoomSwipe> GetByUserId(int userId)
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<RoomSwipe>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       user_id,
                       room_id,
                       is_liked,
                       created_at
                from room_swipes
                where user_id = @user_id
                order by created_at desc";
            cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new RoomSwipe(Convert.ToInt32(data["id"]))
                    {
                        UserId    = Convert.ToInt32(data["user_id"]),
                        RoomId    = Convert.ToInt32(data["room_id"]),
                        IsLiked   = Convert.ToBoolean(data["is_liked"]),
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

    public List<RoomSwipe> GetByRoomId(int roomId)
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<RoomSwipe>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       user_id,
                       room_id,
                       is_liked,
                       created_at
                from room_swipes
                where room_id = @room_id
                order by created_at desc";
            cmd.Parameters.Add("@room_id", NpgsqlDbType.Integer).Value = roomId;

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new RoomSwipe(Convert.ToInt32(data["id"]))
                    {
                        UserId    = Convert.ToInt32(data["user_id"]),
                        RoomId    = Convert.ToInt32(data["room_id"]),
                        IsLiked   = Convert.ToBoolean(data["is_liked"]),
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

    public bool Insert(RoomSwipe swipe)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                insert into room_swipes
                    (user_id,
                     room_id,
                     is_liked)
                values
                    (@user_id,
                     @room_id,
                     @is_liked)
                on conflict (user_id, room_id) do update
                    set is_liked = excluded.is_liked,
                        created_at = now();";

            cmd.Parameters.AddWithValue("@user_id",  NpgsqlDbType.Integer, swipe.UserId);
            cmd.Parameters.AddWithValue("@room_id",  NpgsqlDbType.Integer, swipe.RoomId);
            cmd.Parameters.AddWithValue("@is_liked", NpgsqlDbType.Boolean, swipe.IsLiked);

            return InsertData(dbConn, cmd);
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
            cmd.CommandText = "delete from room_swipes where id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }
}
