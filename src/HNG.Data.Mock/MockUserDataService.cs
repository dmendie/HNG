using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Data;
using System.Data;

namespace HNG.Data.Mock
{
    public class MockUserDataService : IUserDataService
    {
        List<User> Users = new List<User>();
        List<Organisation> Organisations = new List<Organisation>();
        List<UserOrganisation> UserOrganisations = new List<UserOrganisation>();

        public MockUserDataService()
        {
            SetupData();
        }

        public Task<User?> GetByEmailAddress(IDbTransaction? transaction, string EmailAddress)
        {
            var data = Users.FirstOrDefault(x => x.Email == EmailAddress);
            return Task.FromResult(data);
        }

        public Task<User?> GetById(IDbTransaction? transaction, string UserId)
        {
            var data = Users.FirstOrDefault(x => x.UserId == UserId);
            return Task.FromResult(data);
        }

        public Task<IEnumerable<Organisation>> GetOrganisations(IDbTransaction? transaction, string UserId)
        {
            var data = from org in Organisations
                       join userOrg in UserOrganisations on org.OrgId equals userOrg.OrgId
                       where userOrg.UserId == UserId
                       select org;
            return Task.FromResult(data);
        }

        public Task<string> InsertUser(IDbTransaction? transaction, string FirstName, string LastName, string Phone, string EmailAddress, string Password, string CreatedBy)
        {
            var newUserId = Guid.NewGuid().ToString();
            var newOrgId = Guid.NewGuid().ToString();

            Users.Add(new User { UserId = newUserId, FirstName = FirstName, LastName = LastName, Phone = Phone, Email = EmailAddress, Status = Abstractions.Enums.DefaultStatusType.Active, ModifiedBy = CreatedBy, CreatedBy = CreatedBy, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            Organisations.Add(new Organisation { OrgId = newOrgId, Name = $"{FirstName}'s Organisation", Status = Abstractions.Enums.DefaultStatusType.Active, Description = "Yo new Organisation", ModifiedBy = CreatedBy, CreatedBy = CreatedBy, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            UserOrganisations.Add(new UserOrganisation { UserId = newUserId, OrgId = newOrgId });

            return Task.FromResult(newUserId);
        }

        public Task<User?> SearchUser(IDbTransaction? transaction, string SearchUserId, string UserId)
        {
            var data = (from usr in Users
                        join org1 in UserOrganisations on usr.UserId equals org1.UserId
                        join org2 in UserOrganisations on org1.OrgId equals org2.OrgId
                        where org1.UserId == SearchUserId && org2.UserId == UserId
                        select usr).FirstOrDefault();
            return Task.FromResult(data);
        }

        public Task UpdatePassword(IDbTransaction? transaction, string UserId, string Password)
        {
            return Task.CompletedTask;
        }

        void SetupData()
        {
            Users = new List<User>()
            {
                new User()
                {
                    UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e",
                    Email = "user1@mail.com",
                    FirstName = "Deborah",
                    LastName = "Smith",
                    Phone = "2347869574312",
                    Status = Abstractions.Enums.DefaultStatusType.Active,
                    CreatedBy = "User",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "User",
                    ModifiedOn = DateTime.Now

                },
                new User()
                {
                    UserId = "9F4614CF-12A2-4B83-B103-B53361630CF9",
                    Email = "user2@mail.com",
                    FirstName = "Kingslwy",
                    LastName = "Friday",
                    Phone = "2347869574312",
                    Status = Abstractions.Enums.DefaultStatusType.Active,
                    CreatedBy = "User",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "User",
                    ModifiedOn = DateTime.Now

                },
                new User()
                {
                    UserId = Guid.NewGuid().ToString(),
                    Email = "user3@mail.com",
                    FirstName = "Samuel",
                    LastName = "John",
                    Phone = "2347869574312",
                    Status = Abstractions.Enums.DefaultStatusType.Disabled,
                    CreatedBy = "User",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "User",
                    ModifiedOn = DateTime.Now

                }
            };

            Organisations = new List<Organisation>()
            {
                new Organisation()
                {
                     OrgId = "89E75A35-A8E0-4B17-B89A-E4E929B0929C",
                     Name = "John's Organisation",
                     Description= "Monday Hammer Company Co",
                     CreatedBy = "Test",
                     CreatedOn = DateTime.Now,
                     ModifiedBy= "Test",
                     ModifiedOn= DateTime.Now,
                     Status = Abstractions.Enums.DefaultStatusType.Active
                },
                new Organisation()
                {
                     OrgId = "7D6A068C-16CE-4DF7-9E90-F9E4014F4B89",
                     Name = "John's Organisation",
                     Description= "Monday Hammer Company Co",
                     CreatedBy = "Test",
                     CreatedOn = DateTime.Now,
                     ModifiedBy= "Test",
                     ModifiedOn= DateTime.Now,
                     Status = Abstractions.Enums.DefaultStatusType.Active
                },
                new Organisation()
                {
                     OrgId = "16CE-4DF7-9E90-F9E4014F4B89",
                     Name = "John's Organisation",
                     Description= "Monday Hammer Company Co",
                     CreatedBy = "Test",
                     CreatedOn = DateTime.Now,
                     ModifiedBy= "Test",
                     ModifiedOn= DateTime.Now,
                     Status = Abstractions.Enums.DefaultStatusType.Active
                }
            };

            UserOrganisations = new List<UserOrganisation>()
            {
                new UserOrganisation()
                {
                    UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e",
                    OrgId = "89E75A35-A8E0-4B17-B89A-E4E929B0929C"
                },
                new UserOrganisation()
                {
                    UserId = "9F4614CF-12A2-4B83-B103-B53361630CF9",
                    OrgId = "7D6A068C-16CE-4DF7-9E90-F9E4014F4B89"
                },
                new UserOrganisation()
                {
                    UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e",
                    OrgId = "16CE-4DF7-9E90-F9E4014F4B89"
                }
            };
        }
    }
}
