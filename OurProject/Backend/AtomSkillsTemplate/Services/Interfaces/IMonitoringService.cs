using System.Threading.Tasks;

namespace AtomSkillsTemplate.Services.Interfaces
{
    public interface IMonitoringService
    {
        public Task AddRequest(long requestID);
        public void SetupEnvironment();
        public long GetRequestIDThatMachineWorksOn(string machineID);
    }
}
