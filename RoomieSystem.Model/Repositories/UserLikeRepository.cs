using RoomieSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace RoomieSystem.Model.Repositories;

public class UserLikeRepository : BaseRepository
{
    public UserLikeRepository(IConfiguration configuration) : base(configuration) { }

    public List<UserLike> GetAll()
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<UserLike>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       from_user_id,
                       to_user_id,
                       created_at
                from user_likes
                order by created_at desc";

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new UserLike(Convert.ToInt32(data["id"]))
                    {
                        FromUserId = Convert.ToInt32(data["from_user_id"]),
                        ToUserId   = Convert.ToInt32(data["to_user_id"]),
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

    public UserLike? GetById(int id)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       from_user_id,
                       to_user_id,
                       created_at
                from user_likes
                where id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new UserLike(Convert.ToInt32(data["id"]))
                {
                    FromUserId = Convert.ToInt32(data["from_user_id"]),
                    ToUserId   = Convert.ToInt32(data["to_user_id"]),
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

    public List<UserLike> GetByFromUser(int fromUserId)
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<UserLike>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       from_user_id,
                       to_user_id,
                       created_at
                from user_likes
                where from_user_id = @from_user_id
                order by created_at desc";
            cmd.Parameters.Add("@from_user_id", NpgsqlDbType.Integer).Value = fromUserId;

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new UserLike(Convert.ToInt32(data["id"]))
                    {
                        FromUserId = Convert.ToInt32(data["from_user_id"]),
                        ToUserId   = Convert.ToInt32(data["to_user_id"]),
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

    public List<UserLike> GetByToUser(int toUserId)
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<UserLike>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       from_user_id,
                       to_user_id,
                       created_at
                from user_likes
                where to_user_id = @to_user_id
                order by created_at desc";
            cmd.Parameters.Add("@to_user_id", NpgsqlDbType.Integer).Value = toUserId;

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new UserLike(Convert.ToInt32(data["id"]))
                    {
                        FromUserId = Convert.ToInt32(data["from_user_id"]),
                        ToUserId   = Convert.ToInt32(data["to_user_id"]),
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

    // check if A likes B and B likes A (for potential match logic later)
    public bool HasMutualLike(int userAId, int userBId)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select count(*) as cnt
                from user_likes ul1
                join user_likes ul2
                  on ul1.from_user_id = @userA
                 and ul1.to_user_id   = @userB
                 and ul2.from_user_id = @userB
                 and ul2.to_user_id   = @userA";

            cmd.Parameters.Add("@userA", NpgsqlDbType.Integer).Value = userAId;
            cmd.Parameters.Add("@userB", NpgsqlDbType.Integer).Value = userBId;

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                var count = Convert.ToInt32(data["cnt"]);
                return count > 0;
            }

            return false;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool Insert(UserLike like)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                insert into user_likes
                    (from_user_id,
                     to_user_id)
                values
                    (@from_user_id,
                     @to_user_id)
                on conflict (from_user_id, to_user_id) do nothing;";

            cmd.Parameters.AddWithValue("@from_user_id", NpgsqlDbType.Integer, like.FromUserId);
            cmd.Parameters.AddWithValue("@to_user_id",   NpgsqlDbType.Integer, like.ToUserId);

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
            cmd.CommandText = "delete from user_likes where id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }
}
