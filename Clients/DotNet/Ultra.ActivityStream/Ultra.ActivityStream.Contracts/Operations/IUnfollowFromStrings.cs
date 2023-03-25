namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUnfollowFromStrings
    {
        Task UnFollowObject(string Follower, string Followee);
    }
}