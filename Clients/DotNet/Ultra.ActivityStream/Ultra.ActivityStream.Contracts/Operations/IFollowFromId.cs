namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IFollowFromId
    {
        Task FollowObject(Guid Follower, Guid Followee);
    }
}