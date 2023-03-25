using Npgsql;

namespace Ultra.ActivityStream
{
    public interface IActivityStreamClient
    {
        Task ActivityUpsert(string verb, Guid actorId, Guid objectId, Guid? targetId, double latitude, double longitude);
        Task CreateDatabaseObjectsAsync();
        Task ExecuteFunctionAsync(string functionName, NpgsqlParameter[] parameters);
        Task FollowObject(Guid Follower, Guid Followee);
        Task FollowObject(Ultra.ActivityStream.Contracts.IStreamObject Follower, Ultra.ActivityStream.Contracts.IStreamObject Followee);
        Task FollowObject(string Follower, string Followee);
        Task<string> get_activities_by_distance_as_json(double Latitud, double Longitud, int RadiusInMeters, int PageNumber, int PageSize);
        Task<string> GetObjectsByCriteriaAsJson(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize, double? Latitud, double? Longitud, int? RadiusInMeters);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, int PageNumber, int PageSize);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, int PageNumber, int PageSize);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, int PageNumber, int PageSize);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize, double? Latitud, double? Longitud, int? RadiusInMeters);
        Task ObjectStorageUpsert(object Instace);
        Task SimpleTest();
        Task UnFollowObject(Guid Follower, Guid Followee);
        Task UnFollowObject(Ultra.ActivityStream.Contracts.IStreamObject Follower, Ultra.ActivityStream.Contracts.IStreamObject Followee);
        Task UnFollowObject(string Follower, string Followee);
    }
}