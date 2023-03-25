namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUpsertActivityFromIds
    {
        Task ActivityUpsert(string verb, Guid actorId, Guid objectId, Guid? targetId, double latitude, double longitude);
    }
}