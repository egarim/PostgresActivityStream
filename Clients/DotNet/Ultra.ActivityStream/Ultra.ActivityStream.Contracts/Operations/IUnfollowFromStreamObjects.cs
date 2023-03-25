namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUnfollowFromStreamObjects
    {
        Task UnFollowObject(Ultra.ActivityStream.Contracts.IStreamObject Follower, Ultra.ActivityStream.Contracts.IStreamObject Followee);
    }
}