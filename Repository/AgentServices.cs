
using Microsoft.EntityFrameworkCore;
using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.Models;

namespace InsuranceManagementSystem.Services
{
    public class AgentService : IAgentServices
    {
        private readonly DatabaseDbContext _context;

        public AgentService(DatabaseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            return await _context.Agents.Include(a => a.AssignedPolicies).ToListAsync();
        }

        public async Task<Agent> GetAgentByIdAsync(int id)
        {
            return await _context.Agents.Include(a=>a.AssignedPolicies)
                .FirstOrDefaultAsync(a => a.AgentID == id);
        }

        public async Task<Agent> AddAgentAsync(Agent agent)
        {
            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();
            return agent;
        }

        public async Task<Agent> UpdateAgentAsync(int id, Agent agent)
        {
            if (id != agent.AgentID)
            {
                throw new ArgumentException("ID mismatch");
            }

            _context.Entry(agent).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return agent;
        }

        public async Task<bool> DeleteAgentAsync(int id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent == null)
            {
                return false;
            }

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AppointAgentToPolicyAsync(int policyId, int agentId)
        {
            var policy = await _context.Policies.FindAsync(policyId);
            if (policy == null)
            {
                return false;
            }

            policy.AgentID = agentId;
            await _context.SaveChangesAsync();
            return true;
        }

        Task IAgentServices.UpdateAgentAsync(int id, object agent_Name)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Policy>> GetAssignedPoliciesByAgentIdAsync(int agentId)
        {
            var agent = await _context.Agents
                .Include(a => a.AssignedPolicies)

                .FirstOrDefaultAsync(a => a.AgentID == agentId);

            return agent?.AssignedPolicies ?? Enumerable.Empty<Policy>();
        }



    }
}
