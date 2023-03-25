namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IUnfollowObjectFromStrings
    {
        Task UnFollowObjectFromStringsAsync(string Follower, string Followee);
    }
}