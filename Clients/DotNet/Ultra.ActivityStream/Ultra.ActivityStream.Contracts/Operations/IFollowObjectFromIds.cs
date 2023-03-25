namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IFollowObjectFromIds
    {
        Task FollowObjectFromIdsAsync(Guid Follower, Guid Followee);
    }
}