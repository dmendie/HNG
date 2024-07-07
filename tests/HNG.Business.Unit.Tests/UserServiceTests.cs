using HNG.Abstractions.Contracts;
using HNG.Abstractions.Enums;
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
            var service = Build();

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
        public async Task User_Search()
        {
            //arrange
            var service = Build();

            var firstName = "Firstname";
            var lastname = "Lastname";
            var userName = "username";
            var emailAddress = "";
            var status = DefaultStatusType.Active;

            //act
            //var actual = await service.Search<UserDTO?>(firstName, lastname, userName, status, emailAddress);

            ////assert
            //Assert.Multiple(() =>
            //{
            //    Assert.That(actual, Is.Not.Null);
            //});
        }


        [Test]
        public async Task User_Search_Returns_All_Users()
        {
            //arrange
            var service = Build();

            var expectedCount = 1;

            //act
            //var actual = await service.Search<UserDTO?>(null, null, null, null, null);

            ////assert
            //Assert.Multiple(() =>
            //{
            //    Assert.That(actual, Is.Not.Null);
            //    Assert.That(actual?.Count(), Is.GreaterThanOrEqualTo(expectedCount));
            //});
        }

        [Test]
        public async Task User_CreateUser()
        {
            //arrange
            var service = Build();

            var newUser = new UserCreationDTO
            {
                Phone = "newusername",
                Firstname = "namefirst",
                Lastname = "namelast",
                Email = "namefirst@fbn.com",
                Password = "123456"
            };

            //act
            //var actual = await service.CreateUser<UserDTO?>(newUser, GetContext);

            ////assert
            //Assert.Multiple(() =>
            //{
            //    Assert.That(actual, Is.Not.Null);
            //    Assert.That(actual?.FirstName, Is.EqualTo(newUser.Firstname));
            //});
        }

        //[Test]
        //public async Task User_EditUser()
        //{
        //    //arrange
        //    var service = Build();


        //    var userId = 1;
        //    var editUser = new UpdateUserDTO
        //    {
        //        Username = "newUsername",
        //        Firstname = "namefirst",
        //        Lastname = "namelast",
        //        EmailAddress = "namefirst@fbn.com",
        //    };

        //    //act
        //    var actual = await service.UpdateUser<UserDTO?>(userId, editUser, GetContext);

        //    //assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(actual, Is.Not.Null);
        //        Assert.That(actual?.UserName, Is.EqualTo(editUser.Username));
        //    });
        //}

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

        public IUserService Build()
        {
            return DefaultServiceBuilder.Build<IUserService>();
        }
    }
}
