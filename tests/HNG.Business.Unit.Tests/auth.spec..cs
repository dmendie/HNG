using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using HNG.Business.Unit.Tests.Builders;
using SimpleValidator.Exceptions;

namespace HNG.Business.Unit.Tests
{
    public class auth : BaseTest
    {

        [Test]
        [TestCase("paul@mail.com", "Dogg", "oplattye", "08097865343", "123456")]
        [TestCase("pqw@mail.com", "Big Bang", "Stayyer", "08097865343", "ghe55y")]
        [TestCase("mary@mail.com", "Kingley", "ome", "08097865343", "gerg")]
        [TestCase("kong@mail.com", "Bright", "Qwerty1", "08097865343", "ver545")]
        [TestCase("john@mail.com", "Monty", "Freddy", "08097865343", "7878")]
        public async Task User_Register_Successful_And_Contains_User_Details(string email, string firstname, string lastname, string phone, string password)
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
        [TestCase("pliphi@mail.com", "Sarah", "Kingston", "08097865343", "ydeyt56356")]
        [TestCase("oma@mail.com", "Monstal", "Plobg", "08097865343", "Zdfd")]
        [TestCase("paul@mail.com", "Dogg", "oplattye", "08097865343", "123456")]
        [TestCase("pqw@mail.com", "Big Bang", "Stayyer", "08097865343", "ghe55y")]
        [TestCase("mary@mail.com", "Kingley", "ome", "08097865343", "gerg")]
        [TestCase("kong@mail.com", "Bright", "Qwerty", "08097865343", "ver545")]
        [TestCase("john@mail.com", "Monty", "Freddy", "08097865343", "7878")]
        [TestCase("freddy@mail.com", "Fred", "Hart", "08097865343", "563443")]
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

        [Test]
        [TestCase("", "", "", "", "", 6)]
        [TestCase("oma@mail.com", "", "", "", "", 4)]
        [TestCase("paul@mail.com", "Dogg", "", "", "", 3)]
        [TestCase("pqw@mail.com", "Big Bang", "Stayyer", "", "", 2)]
        [TestCase("mary@mail.com", "Kingley", "ome", "08097865343", "", 1)]
        public void User_Registration_Validation_Successful(string email, string firstname, string lastname, string phone, string password, int expectedError)
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

            var expectedErrorCount = expectedError;

            //act
            var exception = Assert.CatchAsync<ValidationException>(async () =>
            {
                var actual = await service.Register<ResponseDataDTO>(model);
            });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(exception.Validator.Errors.Count, Is.EqualTo(expectedErrorCount));
            });
        }

        [Test]
        public async Task User_Registration_Validation_On_Duplicate_Successful()
        {
            //arrange
            var service = BuildUserService();

            var user1Model = new UserCreationDTO { Email = "hngtester@test.com", Firstname = "Hommy", Lastname = "james", Password = "123445", Phone = "08097865433" };
            var user2Model = new UserCreationDTO { Email = "hngtester@test.com", Firstname = "Hommy", Lastname = "james", Password = "123445", Phone = "08097865433" };

            //act
            var actualFirst = await service.Register<ResponseDataDTO>(user1Model);
            var actualSecond = await service.Register<ResponseDataDTO>(user2Model);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actualFirst.status, Is.EqualTo(200));
                Assert.That(actualSecond.status, Is.EqualTo(400));
            });
        }

        [Test]
        public async Task User_Login_Successful_On_Valid_Credentials()
        {
            //arrange
            var service = BuildUserService();

            var model = new UserLoginDTO
            {
                Email = "alpha@example.com",
                Password = "123456",
            };

            //act
            var (status, actual) = await service.LoginUser<ResponseDataDTO>(model);
            var data = actual?.Data as UserTokenDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(status, Is.EqualTo(200));
                Assert.That(data?.AccessToken, Is.Not.Null);
                Assert.That(data?.User.Email, Is.EqualTo(model.Email));
            });
        }

        [Test]
        public async Task User_Login_UnSuccessful_On_InValid_Credentials()
        {
            //arrange
            var service = BuildUserService();

            var model = new UserLoginDTO
            {
                Email = "alpha@example.com",
                Password = "1234",
            };

            //act
            var (status, actual) = await service.LoginUser<ResponseDataDTO>(model);
            var data = actual?.Data as UserTokenDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(status, Is.EqualTo(400));
                Assert.That(data?.AccessToken, Is.Null);
                Assert.That(data?.User?.Email, Is.Null);
            });
        }

        [Test]
        [TestCase("", "", 2)]
        [TestCase("", "123456", 1)]
        [TestCase("alpha@example.com", "", 1)]
        public void User_Login_Validation_Successful_On_Missing_Credentials(string email, string password, int expectedError)
        {
            //arrange
            var service = BuildUserService();
            int expectedErrorCount = expectedError;

            var model = new UserLoginDTO
            {
                Email = email,
                Password = password,
            };

            //act
            var exception = Assert.CatchAsync<ValidationException>(async () =>
            {
                var actual = await service.LoginUser<ResponseDataDTO>(model);
            });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(exception.Validator.Errors.Count, Is.EqualTo(expectedErrorCount));
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
