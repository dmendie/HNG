using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Data;
using System.Data;

namespace HNG.Data.Mock
{
    public class MockOrganisationDataService : IOrganisationDataService
    {
        List<User> Users = new List<User>();
        List<Organisation> Organisations = new List<Organisation>();
        List<UserOrganisation> UserOrganisations = new List<UserOrganisation>();

        public MockOrganisationDataService()
        {
            SetupData();
        }

        public Task AddUser(IDbTransaction? transaction, string UserId, string OrgId, string CreatedBy)
        {
            UserOrganisations.Add(new UserOrganisation { UserId = UserId, OrgId = OrgId });
            return Task.CompletedTask;
        }

        public Task<Organisation?> GetById(IDbTransaction? transaction, string UserId, string OrgId)
        {
            var data = (from org in Organisations
                        join userOrg in UserOrganisations on org.OrgId equals userOrg.OrgId
                        where userOrg.UserId == UserId && userOrg.OrgId == OrgId
                        select org).FirstOrDefault();
            return Task.FromResult(data);
        }

        public Task<string> Insert(IDbTransaction? transaction, string UserId, string Name, string Description, string CreatedBy)
        {
            var newOrgId = Guid.NewGuid().ToString();
            Organisations.Add(new Organisation { OrgId = newOrgId, Name = Name, Status = Abstractions.Enums.DefaultStatusType.Active, Description = Description, ModifiedBy = CreatedBy, CreatedBy = CreatedBy, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            UserOrganisations.Add(new UserOrganisation { UserId = UserId, OrgId = newOrgId });
            return Task.FromResult(newOrgId);
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
                     Description= "Test 1 Company Co",
                     CreatedBy = "Test",
                     CreatedOn = DateTime.Now,
                     ModifiedBy= "Test",
                     ModifiedOn= DateTime.Now,
                     Status = Abstractions.Enums.DefaultStatusType.Active
                },
                new Organisation()
                {
                     OrgId = "7D6A068C-16CE-4DF7-9E90-F9E4014F4B89",
                     Name = "Matinez's Organisation",
                     Description= "Test 2 Company Co",
                     CreatedBy = "Test",
                     CreatedOn = DateTime.Now,
                     ModifiedBy= "Test",
                     ModifiedOn= DateTime.Now,
                     Status = Abstractions.Enums.DefaultStatusType.Active
                },
                new Organisation()
                {
                     OrgId = "7D6A468C-16CE-4DF7-9E90F9E4014F4B89",
                     Name = "Friday's Organisation",
                     Description= "The Bobs",
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
                    OrgId = "7D6A468C-16CE-4DF7-9E90F9E4014F4B89"
                },
                new UserOrganisation()
                {
                    UserId = "7acbba30-a989-4aa4-c702-08db3920bd4e",
                    OrgId = "7D6A068C-16CE-4DF7-9E90-F9E4014F4B89"
                }
            };
        }
    }
}
