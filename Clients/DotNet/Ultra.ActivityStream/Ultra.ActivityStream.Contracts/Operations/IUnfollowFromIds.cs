namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUnfollowFromIds
    {
        Task UnfollowFromIdsAsync(Guid Follower, Guid Followee);
    }
}