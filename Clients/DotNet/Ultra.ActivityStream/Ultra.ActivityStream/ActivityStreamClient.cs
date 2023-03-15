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

        public async Task FollowObject(Guid Follower, Guid Followee)
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
        public async Task UnFollowObject(Guid Follower, Guid Followee)
        {

            using var command = _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;

            string query = "SELECT as_unfollow_object(@follower, @followee)";
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
            IStreamObject streamObject = (IStreamObject)Instace;
           

            var command = ObjectStorageUpsertCore(Instace, streamObject);

            await _dbContext.Database.OpenConnectionAsync();
            var Result = await command.ExecuteScalarAsync();
            _dbContext.Database.CloseConnection();
        }

        protected NpgsqlCommand ObjectStorageUpsertCore(object Instace, IStreamObject streamObject)
        {
            NpgsqlCommand? command = GetSqlCommand();
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
            return command;
        }
        #region GetObjectsByCriteriaAsObjects overloads

        public async Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, int PageNumber, int PageSize)
        {
            var ObjectsJson = await this.GetObjectsByCriteriaAsJson(ObjectType, null, null, null, PageNumber, PageSize, null, null, null);

            var Objects = JsonSerializer.Deserialize<T>(ObjectsJson);
            return Objects;

        }
        public async Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, int PageNumber, int PageSize)
        {
            var ObjectsJson = await this.GetObjectsByCriteriaAsJson(ObjectType, CreatedAt, null, null, PageNumber, PageSize, null, null, null);

            var Objects = JsonSerializer.Deserialize<T>(ObjectsJson);
            return Objects;

        }
        public async Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, int PageNumber, int PageSize)
        {
            var ObjectsJson = await this.GetObjectsByCriteriaAsJson(ObjectType, CreatedAt, UpdatedAt, null, PageNumber, PageSize, null, null, null);

            var Objects = JsonSerializer.Deserialize<T>(ObjectsJson);
            return Objects;

        }
        public async Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize)
        {
            var ObjectsJson = await this.GetObjectsByCriteriaAsJson(ObjectType, CreatedAt, UpdatedAt, ObjectDataWhere, PageNumber, PageSize, null, null, null);

            var Objects = JsonSerializer.Deserialize<T>(ObjectsJson);
            return Objects;

        }
        public async Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize, double? Latitud, double? Longitud, int? RadiusInMeters)
        {
            var ObjectsJson = await this.GetObjectsByCriteriaAsJson(ObjectType, CreatedAt, UpdatedAt, ObjectDataWhere, PageNumber, PageSize, Latitud, Longitud, RadiusInMeters);

            var Objects = JsonSerializer.Deserialize<T>(ObjectsJson);
            return Objects;

        }

        #endregion
        public async Task<string> GetObjectsByCriteriaAsJson(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize, double? Latitud, double? Longitud, int? RadiusInMeters)
        {
            using var command = _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;
            command.CommandText = "SELECT * FROM as_get_objects_by_criteria_as_json(@ObjectType, @CreatedAt, @UpdatedAt, @ObjectDataWhere, @PageNumber, @PageSize, @LocationRadious);";
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@ObjectType", NpgsqlTypes.NpgsqlDbType.Text, ObjectType);
            command.Parameters.AddWithValue("@CreatedAt", NpgsqlTypes.NpgsqlDbType.Date, CreatedAt.HasValue ? (object)CreatedAt.Value : DBNull.Value);
            command.Parameters.AddWithValue("@UpdatedAt", NpgsqlTypes.NpgsqlDbType.Date, UpdatedAt.HasValue ? (object)UpdatedAt.Value : DBNull.Value);

            if (ObjectDataWhere != null)
                command.Parameters.AddWithValue("@ObjectDataWhere", NpgsqlTypes.NpgsqlDbType.Text, ObjectDataWhere);
            else
                command.Parameters.AddWithValue("@ObjectDataWhere", NpgsqlTypes.NpgsqlDbType.Text, DBNull.Value);

            command.Parameters.AddWithValue("@PageNumber", NpgsqlTypes.NpgsqlDbType.Integer, PageNumber);
            command.Parameters.AddWithValue("@PageSize", NpgsqlTypes.NpgsqlDbType.Integer, PageSize);

            if (Latitud.HasValue && Longitud.HasValue & RadiusInMeters.HasValue)
            {
                string locationRadious = $"{Latitud},{Longitud},{RadiusInMeters}";
                command.Parameters.AddWithValue("@LocationRadious", NpgsqlTypes.NpgsqlDbType.Text, locationRadious);
            }
            else
            {
                command.Parameters.AddWithValue("@LocationRadious", NpgsqlTypes.NpgsqlDbType.Text, DBNull.Value);
            }



            await _dbContext.Database.OpenConnectionAsync();
            var Result = await command.ExecuteScalarAsync() as string;
            Debug.WriteLine(Result);
            _dbContext.Database.CloseConnection();
            return Result;
        }



        public async Task ActivityUpsert(string verb, Guid actorId, Guid objectId, Guid? targetId, double latitude, double longitude)
        {
            

            var command=ActivityUpsertCore(verb, actorId, objectId, targetId, latitude, longitude);

            await _dbContext.Database.OpenConnectionAsync();
            var Result = await command.ExecuteNonQueryAsync();
            _dbContext.Database.CloseConnection();
        }

        protected NpgsqlCommand ActivityUpsertCore(string verb, Guid actorId, Guid objectId, Guid? targetId, double latitude, double longitude)
        {
            NpgsqlCommand? command = GetSqlCommand();
            string query = "SELECT as_upsert_activity(gen_random_uuid(), @verb, @actorId, @objectId, @targetId, @latitude, @longitude);";
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@verb", NpgsqlTypes.NpgsqlDbType.Text, verb);
            command.Parameters.AddWithValue("@actorId", NpgsqlTypes.NpgsqlDbType.Uuid, actorId);
            command.Parameters.AddWithValue("@objectId", NpgsqlTypes.NpgsqlDbType.Uuid, objectId);
            command.Parameters.AddWithValue("@targetId", NpgsqlTypes.NpgsqlDbType.Uuid, targetId.HasValue ? (object)targetId.Value : DBNull.Value);
            command.Parameters.AddWithValue("@latitude", NpgsqlTypes.NpgsqlDbType.Numeric, latitude);
            command.Parameters.AddWithValue("@longitude", NpgsqlTypes.NpgsqlDbType.Numeric, longitude);
            return command;
        }

        private NpgsqlCommand? GetSqlCommand()
        {
            return _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;
        }

        public async Task SimpleTest()
        {

            using var command = _dbContext.Database.GetDbConnection().CreateCommand() as NpgsqlCommand;
            //SELECT * FROM as_get_objects_by_criteria_query('user', '2022-01-01', NULL, '"name" = "Bob"', 1, 10, '59.942800,30.307100,1000');
            //string query = "SELECT as_upsert_objectstorage(@object_id, @latitude, @longitude, @object_type, @object_data)";
            command.CommandText = "SELECT * FROM as_get_objects_by_criteria_as_json('user', '2021-01-01', NULL, NULL, 1, 10, '59.942800,30.307100,10000');";
            command.CommandType = CommandType.Text;
            //command.Parameters.AddWithValue("@object_id", NpgsqlTypes.NpgsqlDbType.Uuid, streamObject.Id);
            //command.Parameters.AddWithValue("@latitude", NpgsqlTypes.NpgsqlDbType.Numeric, streamObject.Latitude);
            //command.Parameters.AddWithValue("@longitude", NpgsqlTypes.NpgsqlDbType.Numeric, streamObject.Longitude);
            //command.Parameters.AddWithValue("@object_type", NpgsqlTypes.NpgsqlDbType.Text, streamObject.ObjectType);
            //var Data = JsonSerializer.Serialize(Instace);
            //Debug.WriteLine(Data);
            //command.Parameters.AddWithValue("@object_data", NpgsqlTypes.NpgsqlDbType.Jsonb, Data);







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