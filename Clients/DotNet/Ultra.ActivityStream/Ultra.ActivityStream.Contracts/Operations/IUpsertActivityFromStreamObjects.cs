namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUpsertActivityFromStreamObjects
    {
        Task ActivityUpsert(string verb, IStreamObject actorId, IStreamObject objectId, IStreamObject? targetId, double latitude, double longitude);
    }
}