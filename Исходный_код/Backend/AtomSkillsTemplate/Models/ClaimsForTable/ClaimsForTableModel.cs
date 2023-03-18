using System;
using AtomSkillsTemplate.Models;

namespace AtomSkillsTemplate.Models.ClaimsForTable
{
    public class ClaimsForTableModel
    {
        public long ID { get; set; }
        public DateTime CreateDate { get; set; }
        public int IdClaim { get; set; }
        public string TypeClaim { get; set; }
        public string Text { get; set; }
        public int IdPriority { get; set; }
        public string Priorities { get; set; }
        public int TimeAccordingSla { get; set; }
        public int IdState { get; set; }
        public string State { get; set; }
        public string PlaceOfService { get; set; }
        public  DateTime? DateTimeEditState { get; set; }
        public DateTime? DateTimeCloseClaim { get; set; }
        public string Author { get; set; }
        public string Executor { get; set; }
        public int? IdExecutor { get; set; }
        public string Comment { get; set; }

    }
}
