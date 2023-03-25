
namespace Ultra.ActivityStream.Contracts.Operations
{
    public interface IFollowFromStreamObjects
    {

        Task FollowObject(IStreamObject Follower, IStreamObject Followee);

    }
}