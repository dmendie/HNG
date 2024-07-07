using HNG.Abstractions.Contracts;
using HNG.Abstractions.Services.Business;
using HNG.Business.Unit.Tests.Builders;
using SimpleValidator.Exceptions;

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
            var data = actual.Data as OrganisationDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(data?.Name, Is.Not.Null);
                Assert.That(data?.Description, Is.Not.Null);
            });
        }

        [Test]
        public async Task Organisation_GetById_Not_Valid_Organisation()
        {
            //arrange
            var service = BuildService();

            var UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e";
            var OrgId = "Not-Valid-organisation-id";

            ////act
            var actual = await service.GetById<ResponseDataDTO>(UserId, OrgId);
            var data = actual.Data as OrganisationDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(data?.Name, Is.Null);
                Assert.That(data?.Description, Is.Null);
            });
        }

        [Test]
        public void Organisation_GetById_Not_AccessGranted_Organisation()
        {
            //arrange
            var service = BuildService();

            var UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e";
            var NoAccess_OrgId = "7D6A468C-16CE-4DF7-9E90F9EF4B89";

            ////act
            //assert
            var exception = Assert.CatchAsync<AccessViolationException>(async () =>
            {
                var actual = await service.GetById<ResponseDataDTO>(UserId, NoAccess_OrgId);
            });
        }

        [Test]
        public async Task Organisation_Create_Successful()
        {
            //arrange
            var service = BuildService();

            var model = new OrgCreationDTO
            {
                Description = "from test",
                Name = "test company"
            };

            ////act
            var actual = await service.Create<ResponseDataDTO>(model, UserContext);
            var data = actual.Data as OrganisationDTO;

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(data?.Name, Is.Not.Null);
                Assert.That(data?.Description, Is.Not.Null);
            });
        }

        [Test]
        public void Organisation_Create_UnSuccessful_ValidationError()
        {
            //arrange
            var service = BuildService();
            int expectedErrorCount = 2;

            var model = new OrgCreationDTO
            {
                Description = "",
                Name = ""
            };

            //act
            var exception = Assert.CatchAsync<ValidationException>(async () =>
            {
                var actual = await service.Create<ResponseDataDTO>(model, UserContext);
            });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(exception.Validator.Errors.Count, Is.EqualTo(expectedErrorCount));
                Assert.That(exception.Validator.ErrorByName(nameof(model.Description))[0].Message, Is.EqualTo("'Description' cannot be null or empty."));
                Assert.That(exception.Validator.ErrorByName(nameof(model.Name))[0].Message, Is.EqualTo("'Name' cannot be null or empty."));
            });
        }

        [Test]
        public void Organisation_AddUser_UnSuccessful_ValidationError()
        {
            //arrange
            var service = BuildService();
            int expectedErrorCount = 1;
            var OrgId = "some-random-id-for-validatiion-test";

            var model = new OrgUserDTO
            {
                UserId = ""
            };

            //act
            var exception = Assert.CatchAsync<ValidationException>(async () =>
            {
                var actual = await service.AddUserToOrg(OrgId, model, UserContext);
            });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(exception.Validator.Errors.Count, Is.EqualTo(expectedErrorCount));
                Assert.That(exception.Validator.ErrorByName(nameof(model.UserId))[0].Message, Is.EqualTo("A valid user identifier is required*"));
            });
        }

        [Test]
        public void Organisation_AddUser_Not_AccessGranted_Organisation()
        {
            //arrange
            var service = BuildService();

            var NoAccess_OrgId = "some-random-id-for-validatiion-test"; ;

            var model = new OrgUserDTO
            {
                UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e"
            };

            ////act
            //assert
            var exception = Assert.CatchAsync<AccessViolationException>(async () =>
            {
                var actual = await service.AddUserToOrg(NoAccess_OrgId, model, UserContext);
            });
        }

        [Test]
        public async Task Organisation_AddUser_Successful()
        {
            //arrange
            var service = BuildService();

            var Access_OrgId = "89E75A35-A8E0-4B17-B89A-E4E929B0929C"; ;

            var model = new OrgUserDTO
            {
                UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e"
            };

            ////act
            var actual = await service.AddUserToOrg(Access_OrgId, model, UserContext);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Status, Is.EqualTo("success"));
            });
        }


        public IOrganisationService BuildService()
        {
            return DefaultServiceBuilder.Build<IOrganisationService>();
        }
    }
}
