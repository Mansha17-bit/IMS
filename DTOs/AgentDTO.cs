namespace InsuranceManagementSystem.DTOs
{
    public class AgentDTO
    {
         public string? Agent_Name { get; set; }
        public string? ContactInfo { get; set; }
        public ICollection<Policy>? AssignedPolicies { get; internal set; }
    }
}
