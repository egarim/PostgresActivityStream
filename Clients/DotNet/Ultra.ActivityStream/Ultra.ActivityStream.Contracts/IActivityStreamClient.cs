﻿using Ultra.ActivityStream.Contracts.Operations;

namespace Ultra.ActivityStream.Contracts
{
    public interface IActivityStreamClient :
        IFollowObjectFromStrings, IFollowObjectFromIds, IFollowObjectFromStreamObjects,
        IUnfollowObjectFromStrings, IUnfollowObjectFromIds, IUnfollowFromStreamObjects,
        IUpsertObjectFromStreamObject,
        IUpsertActivityFromIds
    {
     
        Task CreateDatabaseObjectsAsync();
        //Task ExecuteFunctionAsync(string functionName, NpgsqlParameter[] parameters);

        Task<string> get_activities_by_distance_as_json(double Latitud, double Longitud, int RadiusInMeters, int PageNumber, int PageSize);
        Task<string> GetObjectsByCriteriaAsJson(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize, double? Latitud, double? Longitud, int? RadiusInMeters);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, int PageNumber, int PageSize);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, int PageNumber, int PageSize);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, int PageNumber, int PageSize);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize);
        Task<T> GetObjectsByCriteriaAsObjects<T>(string ObjectType, DateTime? CreatedAt, DateTime? UpdatedAt, string? ObjectDataWhere, int PageNumber, int PageSize, double? Latitud, double? Longitud, int? RadiusInMeters);
       
        Task SimpleTest();



    }
}