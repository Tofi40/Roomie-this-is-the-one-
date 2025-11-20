using RoomieSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace RoomieSystem.Model.Repositories;

public class MatchRepository : BaseRepository
{
    public MatchRepository(IConfiguration configuration) : base(configuration) { }

    public List<Match> GetAll()
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<Match>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       user1_id,
                       user2_id,
                       room_id,
                       matched_at
                from matches
                order by matched_at desc";

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new Match(Convert.ToInt32(data["id"]))
                    {
                        User1Id   = Convert.ToInt32(data["user1_id"]),
                        User2Id   = Convert.ToInt32(data["user2_id"]),
                        RoomId    = Convert.ToInt32(data["room_id"]),
                        MatchedAt = Convert.ToDateTime(data["matched_at"])
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

    public Match? GetById(int id)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       user1_id,
                       user2_id,
                       room_id,
                       matched_at
                from matches
                where id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new Match(Convert.ToInt32(data["id"]))
                {
                    User1Id   = Convert.ToInt32(data["user1_id"]),
                    User2Id   = Convert.ToInt32(data["user2_id"]),
                    RoomId    = Convert.ToInt32(data["room_id"]),
                    MatchedAt = Convert.ToDateTime(data["matched_at"])
                };
            }

            return null;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public List<Match> GetByUserId(int userId)
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<Match>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       user1_id,
                       user2_id,
                       room_id,
                       matched_at
                from matches
                where user1_id = @user_id
                   or user2_id = @user_id
                order by matched_at desc";
            cmd.Parameters.Add("@user_id", NpgsqlDbType.Integer).Value = userId;

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new Match(Convert.ToInt32(data["id"]))
                    {
                        User1Id   = Convert.ToInt32(data["user1_id"]),
                        User2Id   = Convert.ToInt32(data["user2_id"]),
                        RoomId    = Convert.ToInt32(data["room_id"]),
                        MatchedAt = Convert.ToDateTime(data["matched_at"])
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

    public List<Match> GetByRoomId(int roomId)
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<Match>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       user1_id,
                       user2_id,
                       room_id,
                       matched_at
                from matches
                where room_id = @room_id
                order by matched_at desc";
            cmd.Parameters.Add("@room_id", NpgsqlDbType.Integer).Value = roomId;

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new Match(Convert.ToInt32(data["id"]))
                    {
                        User1Id   = Convert.ToInt32(data["user1_id"]),
                        User2Id   = Convert.ToInt32(data["user2_id"]),
                        RoomId    = Convert.ToInt32(data["room_id"]),
                        MatchedAt = Convert.ToDateTime(data["matched_at"])
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

    public bool Insert(Match m)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                insert into matches
                    (user1_id,
                     user2_id,
                     room_id)
                values
                    (@user1_id,
                     @user2_id,
                     @room_id)";

            cmd.Parameters.AddWithValue("@user1_id", NpgsqlDbType.Integer, m.User1Id);
            cmd.Parameters.AddWithValue("@user2_id", NpgsqlDbType.Integer, m.User2Id);
            cmd.Parameters.AddWithValue("@room_id",  NpgsqlDbType.Integer, m.RoomId);

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
            cmd.CommandText = "delete from matches where id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }
}
