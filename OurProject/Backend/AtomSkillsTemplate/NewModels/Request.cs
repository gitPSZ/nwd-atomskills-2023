using System;

namespace AtomSkillsTemplate.NewModels
{
    public class Request
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get;set; }
        public long IdContractor { get; set; }
        public string StateCode { get; set; }
        public string StateCaption { get; set; }


    }
}
