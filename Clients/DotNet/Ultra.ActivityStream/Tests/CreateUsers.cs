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
        public void Test1()
        {
            Assert.Pass();
        }
    }
}