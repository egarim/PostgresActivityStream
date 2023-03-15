using Brevitas.AppFramework;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ultra.ActivityStream
{
    public class ActivityStreamClient
    {
        private readonly ActivityStreamDbContext _dbContext;

        public ActivityStreamClient(ActivityStreamDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void MethodName()
        {
            //string query = "SELECT as_upsert_objectstorage(@object_id, @latitude, @longitude, @object_type, @object_data)";
            //using var command = _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;
            //using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            //{

            //}
        }
        public async Task CreateDatabaseObjectsAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Ultra.ActivityStream.ActivityStream.sql";
            string SqlScript = "";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                SqlScript = reader.ReadToEnd();
            }

            using var command = _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;

            await _dbContext.Database.OpenConnectionAsync();
            command.CommandText = SqlScript;
            command.CommandType = CommandType.Text;
            var Result = await command.ExecuteNonQueryAsync();
            _dbContext.Database.CloseConnection();
        }
        public async Task FollowObject(string Follower, string Followee)
        {
            await FollowObject(Guid.Parse(Follower), Guid.Parse(Followee));
        }

        public async Task FollowObject(Guid Follower,Guid Followee)
        {
            
            using var command = _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;

            string query = "SELECT as_follow_object(@follower, @followee)";
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@follower", NpgsqlTypes.NpgsqlDbType.Uuid, Follower);
            command.Parameters.AddWithValue("@followee", NpgsqlTypes.NpgsqlDbType.Uuid, Followee);
            await _dbContext.Database.OpenConnectionAsync();
            var Result = await command.ExecuteScalarAsync();
            _dbContext.Database.CloseConnection();
        }
        public async Task ObjectStorageUpsert(object Instace)
        {
            IStreamObject streamObject=(IStreamObject)Instace;
            using var command = _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;

            string query = "SELECT as_upsert_objectstorage(@object_id, @latitude, @longitude, @object_type, @object_data)";
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@object_id", NpgsqlTypes.NpgsqlDbType.Uuid, streamObject.Id);
            command.Parameters.AddWithValue("@latitude", NpgsqlTypes.NpgsqlDbType.Numeric, streamObject.Latitude);
            command.Parameters.AddWithValue("@longitude", NpgsqlTypes.NpgsqlDbType.Numeric, streamObject.Longitude);
            command.Parameters.AddWithValue("@object_type", NpgsqlTypes.NpgsqlDbType.Text, streamObject.ObjectType);
            var Data = JsonSerializer.Serialize(Instace);
            Debug.WriteLine(Data);
            command.Parameters.AddWithValue("@object_data", NpgsqlTypes.NpgsqlDbType.Jsonb, Data);







            await _dbContext.Database.OpenConnectionAsync();
            var Result = await command.ExecuteScalarAsync();
            _dbContext.Database.CloseConnection();
        }
        public async Task ExecuteFunctionAsync(string functionName, NpgsqlParameter[] parameters)
        {
            using var command = _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;

            //command.CommandText = functionName;
            //command.CommandType = CommandType.Text;
            //command.Parameters.AddRange(parameters);

            string query = "SELECT as_upsert_objectstorage(@object_id, @latitude, @longitude, @object_type, @object_data)";
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@object_id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
            command.Parameters.AddWithValue("@latitude", NpgsqlTypes.NpgsqlDbType.Numeric, 59.9311);
            command.Parameters.AddWithValue("@longitude", NpgsqlTypes.NpgsqlDbType.Numeric, 30.3609);
            command.Parameters.AddWithValue("@object_type", NpgsqlTypes.NpgsqlDbType.Text, "user");
            command.Parameters.AddWithValue("@object_data", NpgsqlTypes.NpgsqlDbType.Jsonb, "{\"name\": \"Alice\", \"age\": 27}");
            //command.Parameters.AddWithValue("@object_data",NpgsqlTypes.NpgsqlDbType.Jsonb, "{\"name\": \"Alice\", \"age\": 27, \"email\": \"alice@example.com\", \"picture_url\": \"https://example.com/pictures/alice.jpg\"}");






            await _dbContext.Database.OpenConnectionAsync();
            var Result = await command.ExecuteScalarAsync();
            _dbContext.Database.CloseConnection();
        }
    }
}