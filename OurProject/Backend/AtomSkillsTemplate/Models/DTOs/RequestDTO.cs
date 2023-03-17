using System;
using AtomSkillsTemplate.Models;

namespace AtomSkillsTemplate.Models.DTOs
{
    public class RequestDTO
    {
        public long ID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? IdType { get; set; }
        public string Text { get; set; }
        public string placeOfService { get; set; }
        public int? IdExecutor { get; set; }
        public int? IdPriority { get; set; }
        public int? IdAuthor { get; set; }
    }
}
