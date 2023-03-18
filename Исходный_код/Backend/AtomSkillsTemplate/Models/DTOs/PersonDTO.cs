using AtomSkillsTemplate.Models;

namespace AtomSkillsTemplate.Models.DTOs
{
    public class PersonDTO
    {
        public long ID { get; set; }
        public string NameClient { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }


    }
}
