
namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IFollowObjectFromStreamObjects
    {

        Task FollowObjectFromStreamObjectsAsync(IStreamObject Follower, IStreamObject Followee);

    }
}