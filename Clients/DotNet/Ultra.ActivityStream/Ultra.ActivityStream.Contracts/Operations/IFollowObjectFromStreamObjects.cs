
namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IFollowObjectFromStreamObjects
    {

        Task FollowObject(IStreamObject Follower, IStreamObject Followee);

    }
}