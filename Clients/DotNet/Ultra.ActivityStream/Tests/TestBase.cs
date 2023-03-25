using Ultra.ActivityStream;

namespace Tests
{
    public abstract class TestBase
    {
        private string CnxTemplate = "Host=localhost;Database=$DatabaseName;Username=postgres;Password=1234567890";
        public abstract string SetConnectionStringName();

        protected ActivityStreamClient activityStreamClient;
        [SetUp]
        public virtual async Task Setup()
        {

            ActivityStreamDbContext context = new ActivityStreamDbContext(CnxTemplate.Replace("$DatabaseName", SetConnectionStringName()));
            activityStreamClient = new ActivityStreamClient(context);
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //await activityStreamClient.CreateDatabaseObjectsAsync();


        }
    }
}