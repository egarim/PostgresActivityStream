namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUpsertActivityFromStrings

    {
        Task ActivityUpsert(string verb, string actorId, string objectId, string? targetId, double latitude, double longitude);
    }
}