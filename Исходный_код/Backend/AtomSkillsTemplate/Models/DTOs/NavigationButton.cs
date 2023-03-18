using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtomSkillsTemplate.Models.DTOs
{
    public class NavigationButton
    {
        public long Id { get; set; }
        public string RouterLink { get; set; }
        public string IconClass { get; set; }
        public string Caption { get; set; }
        public long RoleId { get; set; }
    }
}
