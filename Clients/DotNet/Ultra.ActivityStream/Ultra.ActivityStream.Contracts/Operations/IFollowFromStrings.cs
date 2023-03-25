namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IFollowFromStrings
    {
        Task FollowObject(string Follower, string Followee);
    }
}