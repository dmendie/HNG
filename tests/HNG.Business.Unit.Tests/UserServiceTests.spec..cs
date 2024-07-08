using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using HNG.Business.Unit.Tests.Builders;

namespace HNG.Business.Unit.Tests
{
    public class UserServiceTests : BaseTest
    {

        [Test]
        public async Task User_GetById()
        {
            //arrange
            var service = BuildUserService();

            var userId = "7acbba30-a989-4aa4-c702-08db3920bd4e";

            ////act
            var actual = await service.GetById<UserDTO?>(userId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
            });
        }

        [Test]
        public async Task User_Register_UnSuccessful()
        {
            //arrange
            var service = BuildUserService();

            var model = new UserCreationDTO
            {
                Email = "user1@mail.com",
                Firstname = "Friday",
                Lastname = "Sona",
                Password = "1234356",
                Phone = "07046512342"
            };

            //act
            var (status, actual) = await service.Register<ResponseDataDTO>(model);
            var data = actual?.Data as UserTokenDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(status, Is.EqualTo(400));
                Assert.That(data?.AccessToken, Is.Null);
            });
        }

        [Test]
        public async Task User_Register_Successful()
        {
            //arrange
            var service = BuildUserService();

            var model = new UserCreationDTO
            {
                Email = "absolutenewuser@mail.com",
                Firstname = "Friday",
                Lastname = "Sona",
                Password = "1234356",
                Phone = "07046512342"
            };

            //act
            var (status, actual) = await service.Register<ResponseDataDTO>(model);
            var data = actual?.Data as UserTokenDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(status, Is.EqualTo(200));
                Assert.That(data?.AccessToken, Is.Not.Null);
            });
        }


        [Test]
        public async Task User_Register_IsNot_Successful()
        {
            //arrange
            var service = BuildUserService();

            var model = new UserCreationDTO
            {
                Email = "user1@mail.com",
                Firstname = "Friday",
                Lastname = "Sona",
                Password = "1234356",
                Phone = "07046512342"
            };

            //act
            var (status, actual) = await service.Register<ResponseDataDTO>(model);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(status, Is.EqualTo(400));
                Assert.That(actual?.Data, Is.Null);
            });
        }

        [Test]
        public async Task Auth_User_Generate_Token()
        {
            //arrange
            var auth = BuildAuthService();

            var model = new UserDTO
            {
                Email = "tester1@mail.com",
                FirstName = "Friday",
                LastName = "Sona",
                Phone = "07046512342",
                UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e"
            };

            //act
            var actual = await auth.CreateToken(model);

            ////assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.AccessToken, Is.Not.Null);
                Assert.That(actual.User, Is.Not.Null);
                Assert.That(actual.User.UserId, Is.EqualTo(model.UserId));
            });
        }

        [Test]
        public async Task User_Get_orgs_successful()
        {
            //arrange
            var service = BuildUserService();
            var UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e";
            int expectedcount = 2;

            //act
            var actual = await service.GetUserOrganisations<ResponseDataDTO>(UserId);
            var data = actual.Data as OrganisationListDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(data.Organisations.Count(), Is.EqualTo(expectedcount));
            });
        }

        //[Test]
        //public async Task User_GetUserGroups()
        //{
        //    //arrange
        //    var service = Build();


        //    var userId = 1;
        //    var expectedCount = 1;

        //    //act
        //    var actual = await service.GetUserGroups<GroupMemberDTO?>(userId);

        //    //assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(actual, Is.Not.Null);
        //        Assert.That(actual.Count, Is.GreaterThanOrEqualTo(expectedCount));
        //    });
        //}

        //[Test]
        //public async Task User_GetUserRoles()
        //{
        //    //arrange
        //    var service = Build();

        //    var roleId = 1;

        //    //act
        //    var actual = await service.GetUserRoles<RoleDTO?>(roleId);

        //    //assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(actual, Is.Not.Null);
        //    });
        //}

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
