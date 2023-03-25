namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IFollowObjectFromStrings
    {
        Task FollowObjectFromStringsAsync(string Follower, string Followee);
    }
}