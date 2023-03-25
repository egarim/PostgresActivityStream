namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUnfollowFromIds
    {
        Task UnFollowObject(Guid Follower, Guid Followee);
    }
}