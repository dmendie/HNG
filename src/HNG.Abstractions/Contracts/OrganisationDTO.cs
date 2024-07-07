namespace HNG.Abstractions.Contracts
{
    public class OrganisationDTO
    {
        public string OrgId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class OrganisationListDTO
    {
        public IEnumerable<OrganisationDTO> Organisations { get; set; } = new List<OrganisationDTO>();
    }
}
