namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUnfollowObjectFromIds
    {
        Task UnfollowObjectFromIdsAsync(Guid Follower, Guid Followee);
    }
}