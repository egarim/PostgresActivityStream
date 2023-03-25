namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IFollowObjectFromStrings
    {
        Task FollowObject(string Follower, string Followee);
    }
}