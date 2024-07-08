using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using HNG.Business.Unit.Tests.Builders;

namespace HNG.Business.Unit.Tests
{
    public class auth : BaseTest
    {

        [Test]
        [TestCase("paul@mail.com", "Dogg", "oplattye 1", "08097865343", "123456")]
        [TestCase("pqw@mail.com", "Big Bang", "Stayyer 1", "08097865343", "ghe55y")]
        [TestCase("mary@mail.com", "Kingley", "ome 1", "08097865343", "gerg")]
        [TestCase("kong@mail.com", "Bright", "Qwerty 1", "08097865343", "ver545")]
        [TestCase("john@mail.com", "Monty", "Freddy 1", "08097865343", "7878")]
        public async Task User_Register_Successful(string email, string firstname, string lastname, string phone, string password)
        {
            //arrange
            var service = BuildUserService();

            var model = new UserCreationDTO
            {
                Email = email,
                Firstname = firstname,
                Lastname = lastname,
                Password = password,
                Phone = phone
            };

            //act
            var (status, actual) = await service.Register<ResponseDataDTO>(model);
            var data = actual?.Data as UserTokenDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(status, Is.EqualTo(200));
                Assert.That(data?.AccessToken, Is.Not.Null);
                Assert.That(data?.User.FirstName, Is.EqualTo(model.Firstname));
                Assert.That(data?.User.LastName, Is.EqualTo(model.Lastname));
                Assert.That(data?.User.Phone, Is.EqualTo(model.Phone));
                Assert.That(data?.User.Email, Is.EqualTo(model.Email));
            });
        }

        [Test]
        [TestCase("pliphi@mail.com", "Sarah", "Kingston 1", "08097865343", "ydeyt56356")]
        [TestCase("oma@mail.com", "Monstal", "Plobg 1", "08097865343", "Zdfd")]
        [TestCase("paul@mail.com", "Dogg", "oplattye 1", "08097865343", "123456")]
        [TestCase("pqw@mail.com", "Big Bang", "Stayyer 1", "08097865343", "ghe55y")]
        [TestCase("mary@mail.com", "Kingley", "ome 1", "08097865343", "gerg")]
        [TestCase("kong@mail.com", "Bright", "Qwerty 1", "08097865343", "ver545")]
        [TestCase("john@mail.com", "Monty", "Freddy 1", "08097865343", "7878")]
        [TestCase("freddy@mail.com", "Fred", "Hart 1", "08097865343", "563443")]
        public async Task User_Default_Org_On_Registration_Successful(string email, string firstname, string lastname, string phone, string password)
        {
            //arrange
            var service = BuildUserService();

            var model = new UserCreationDTO
            {
                Email = email,
                Firstname = firstname,
                Lastname = lastname,
                Password = password,
                Phone = phone
            };

            var expectedOrgName = $"{model.Firstname}'s Organisation";
            var expectedCount = 1;

            //act
            var (status, actual) = await service.Register<ResponseDataDTO>(model);
            var data = actual?.Data as UserTokenDTO;

            var organisationActual = await service.GetUserOrganisations<ResponseDataDTO>(data!.User.UserId);
            var orgData = organisationActual?.Data as OrganisationListDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(status, Is.EqualTo(200));
                Assert.That(orgData.Organisations.Count, Is.EqualTo(expectedCount));
                Assert.That(orgData.Organisations.First().Name, Is.EqualTo(expectedOrgName));
            });
        }


        public IUserService BuildUserService()
        {
            return DefaultServiceBuilder.Build<IUserService>();
        }

        public IAuthUserService BuildAuthService()
        {
            return DefaultServiceBuilder.Build<IAuthUserService>();
        }
    }
}
