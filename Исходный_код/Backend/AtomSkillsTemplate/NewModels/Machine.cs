﻿using System;
using System.Collections.Generic;

namespace AtomSkillsTemplate.NewModels
{
    public class Machine
    {
        public string Id { get; set; }
        public string MachineType { get; set; }
        public string machineTypeCaption { get; set; }
        public long Port { get; set; }
        public string IsDeleted { get; set; }
        public int IdState { get; set; }
        public string State { get; set; }
        public long? IdRequest { get; set; }



    }
}
