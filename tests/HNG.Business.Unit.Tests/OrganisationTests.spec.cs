using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using HNG.Business.Unit.Tests.Builders;

namespace HNG.Business.Unit.Tests
{
    public class Organisation : BaseTest
    {

        [Test]
        public async Task Organisation_GetById_Succesful()
        {
            //arrange
            var service = BuildService();

            var UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e";
            var OrgId = "89E75A35-A8E0-4B17-B89A-E4E929B0929C";

            ////act
            var actual = await service.GetById<ResponseDataDTO>(UserId, OrgId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Data, Is.Not.Null);
            });
        }

        [Test]
        public async Task Organisation_GetById_UnSuccesful()
        {
            //arrange
            var service = BuildService();

            var UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e";
            var OrgId = "A8E0-4B17-B89A-E4E929B0929C";

            ////act
            var actual = await service.GetById<ResponseDataDTO>(UserId, OrgId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Data, Is.Null);
            });
        }



        public IOrganisationService BuildService()
        {
            return DefaultServiceBuilder.Build<IOrganisationService>();
        }
    }
}
