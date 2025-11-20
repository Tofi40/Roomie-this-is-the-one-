using RoomieSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace RoomieSystem.Model.Repositories;

public class MessageRepository : BaseRepository
{
    public MessageRepository(IConfiguration configuration) : base(configuration) { }

    public List<Message> GetByMatchId(int matchId)
    {
        NpgsqlConnection? dbConn = null;
        var list = new List<Message>();

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       match_id,
                       from_user_id,
                       message_text,
                       sent_at
                from messages
                where match_id = @match_id
                order by sent_at";
            cmd.Parameters.Add("@match_id", NpgsqlDbType.Integer).Value = matchId;

            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    list.Add(new Message(Convert.ToInt32(data["id"]))
                    {
                        MatchId     = Convert.ToInt32(data["match_id"]),
                        FromUserId  = Convert.ToInt32(data["from_user_id"]),
                        MessageText = data["message_text"].ToString(),
                        SentAt      = Convert.ToDateTime(data["sent_at"])
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

    public Message? GetById(int id)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                select id,
                       match_id,
                       from_user_id,
                       message_text,
                       sent_at
                from messages
                where id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            var data = GetData(dbConn, cmd);
            if (data != null && data.Read())
            {
                return new Message(Convert.ToInt32(data["id"]))
                {
                    MatchId     = Convert.ToInt32(data["match_id"]),
                    FromUserId  = Convert.ToInt32(data["from_user_id"]),
                    MessageText = data["message_text"].ToString(),
                    SentAt      = Convert.ToDateTime(data["sent_at"])
                };
            }

            return null;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool Insert(Message m)
    {
        NpgsqlConnection? dbConn = null;

        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                insert into messages
                    (match_id,
                     from_user_id,
                     message_text)
                values
                    (@match_id,
                     @from_user_id,
                     @message_text)";

            cmd.Parameters.AddWithValue("@match_id",     NpgsqlDbType.Integer, m.MatchId);
            cmd.Parameters.AddWithValue("@from_user_id", NpgsqlDbType.Integer, m.FromUserId);
            cmd.Parameters.AddWithValue("@message_text", NpgsqlDbType.Text,    m.MessageText);

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
            cmd.CommandText = "delete from messages where id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }
        finally
        {
            dbConn?.Close();
        }
    }
}
