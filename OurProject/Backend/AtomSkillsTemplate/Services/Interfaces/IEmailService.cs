using System.Threading.Tasks;

namespace AtomSkillsTemplate.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
}
