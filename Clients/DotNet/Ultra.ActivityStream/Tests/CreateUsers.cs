using Newtonsoft.Json;
using System.Security.Principal;
using Ultra.ActivityStream;

namespace Tests
{
    public class CreateUserObjects : Tests.Infrastructure.TestBase
    {
       

        public override string SetConnectionStringName()
        {
            return nameof(CreateUserObjects);
        }

        [SetUp]
        public override Task Setup()
        {
            return base.Setup();
        }
        [Test]
        public async Task Test_as_get_activities_by_distance_as_json()
        {
            //TODO test
           var result=   await this.activityStreamClient.get_activities_by_distance_as_json(59.9343, 30.3351, 1000, 1, 10);
        }
        [Test]
        public async Task CreateUsersNearSanSalvadorAndQueryThem()
        {
            const int RadiusDistanceInMeters = 10000;
            var SanSalvadorAccounts = AccountCreator.CreateAccountsNearSanSalvador();
            foreach (Ultra.ActivityStream.Contracts.Account account in SanSalvadorAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }
            var BuenosAiresArgentinaAccounts = AccountCreator.CreateAccountsNearBuenosAires();
            foreach (Ultra.ActivityStream.Contracts.Account account in BuenosAiresArgentinaAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }
            var Page1 = await this.activityStreamClient.GetObjectsByCriteriaAsObjects<List<Ultra.ActivityStream.Contracts.Account>>("user", null, null, null, 1, 5, CityCoordinates.SanSalvadorLocation.Latitude, CityCoordinates.SanSalvadorLocation.Longitude, RadiusDistanceInMeters);
            
            
            
            foreach (Ultra.ActivityStream.Contracts.Account account in Page1)
            {
                Assert.IsTrue(Ultra.ActivityStream.Contracts.GeoHelper.IsWithinRadius(CityCoordinates.SanSalvadorLocation.Latitude, CityCoordinates.SanSalvadorLocation.Longitude, account.Latitude, account.Longitude,10000));

            }


        }


        [Test]
        public async Task CreateUsersTest()
        {
            var SpbAccounts = AccountCreator.CreateAccountsNearStPetersburg();
            foreach (Ultra.ActivityStream.Contracts.Account account in SpbAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }

            var Objects = await this.activityStreamClient.GetObjectsByCriteriaAsJson("user", null, null, null, 1, 10, null, null, null);

            var AccountsFromJson = JsonConvert.DeserializeObject<List<Ultra.ActivityStream.Contracts.Account>>(Objects);

            Assert.AreEqual(5, AccountsFromJson.Count());
        }
        /// <summary>
        /// Create 30 users and the retrive the first 3 pages
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateUsers30UsersAndLoadThemByPages()
        {
            var SpbAccounts = AccountCreator.CreateAccountsNearStPetersburg();
            foreach (Ultra.ActivityStream.Contracts.Account account in SpbAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }
            var GlendaleAccounts = AccountCreator.CreateAccountsNearGlendale();
            foreach (Ultra.ActivityStream.Contracts.Account account in GlendaleAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }
            var SantoDomingoAccounts = AccountCreator.CreateAccountsNearSantoDomingo();
            foreach (Ultra.ActivityStream.Contracts.Account account in SantoDomingoAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }
            var SanSalvadorAccounts = AccountCreator.CreateAccountsNearSanSalvador();
            foreach (Ultra.ActivityStream.Contracts.Account account in SanSalvadorAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }
            var SantiagoDeChileAccounts = AccountCreator.CreateAccountsNearSantiago();
            foreach (Ultra.ActivityStream.Contracts.Account account in SantiagoDeChileAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }
            var BuenosAiresArgentinaAccounts = AccountCreator.CreateAccountsNearBuenosAires();
            foreach (Ultra.ActivityStream.Contracts.Account account in BuenosAiresArgentinaAccounts)
            {
                await this.activityStreamClient.ObjectStorageUpsert(account);
            }
            var Page1 = await this.activityStreamClient.GetObjectsByCriteriaAsObjects<List<Ultra.ActivityStream.Contracts.Account>>("user", null, null, null, 1, 5, null, null, null);
            var Page2 = await this.activityStreamClient.GetObjectsByCriteriaAsObjects<List<Ultra.ActivityStream.Contracts.Account>>("user", null, null, null, 2, 5, null, null, null);
            var Page3 = await this.activityStreamClient.GetObjectsByCriteriaAsObjects<List<Ultra.ActivityStream.Contracts.Account>>("user", null, null, null, 3, 5, null, null, null);

            Assert.AreEqual(5, Page1.Count());
            Assert.AreEqual(5, Page2.Count());
            Assert.AreEqual(5, Page3.Count());
        }
    }
}