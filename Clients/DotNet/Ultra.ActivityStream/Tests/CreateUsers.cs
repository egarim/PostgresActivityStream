using Brevitas.AppFramework;

namespace Tests
{
    public class CreateUsers : TestBase
    {
        public override string SetConnectionStringName()
        {
            return nameof(CreateUsers);
        }

        [SetUp]
        public override Task Setup()
        {
            return base.Setup();
        }

        [Test]
        public async Task CreateUsersTest()
        {
            var SpbAccounts=AccountCreator.CreateAccountsNearStPetersburg();
            foreach (Account account in SpbAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }

           await this.activityStreamClient.FollowObject(SpbAccounts.FirstOrDefault().Id, SpbAccounts.LastOrDefault().Id);
           Assert.Pass();
        }
    }
}