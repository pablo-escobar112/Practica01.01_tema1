using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace InventoryWpfApp
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private NpgsqlConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch { return false; }
        }

        public List<TopicModel> GetTopics()
        {
            var topics = new List<TopicModel>();
            string sql = @"SELECT ""Id"", ""Title"", ""OrderNumber"" FROM public.""Topics"" ORDER BY ""OrderNumber""";

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topics.Add(new TopicModel
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            OrderNumber = reader.GetInt32(2)
                        });
                    }
                }
            }
            return topics;
        }

        public List<ContentBlockModel> GetContentBlocks(int topicId)
        {
            var blocks = new List<ContentBlockModel>();
            string sql = @"SELECT ""Id"", ""TopicId"", ""OrderNumber"", ""Tag"", ""Text""
                           FROM public.""ContentBlocks""
                           WHERE ""TopicId"" = @tid ORDER BY ""OrderNumber""";

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("tid", topicId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            blocks.Add(new ContentBlockModel
                            {
                                Id = reader.GetInt32(0),
                                TopicId = reader.GetInt32(1),
                                OrderNumber = reader.GetInt32(2),
                                Tag = reader.GetString(3),
                                Text = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return blocks;
        }
    }
}