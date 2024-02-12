using AutoFixture;
using AutoFixture.AutoMoq;

namespace BankManagement.Controller.UnitTests
{
    public class BaseTest
    {
        protected readonly IFixture Fixture;

        public BaseTest()
        {
            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization());
        }
    }
}
