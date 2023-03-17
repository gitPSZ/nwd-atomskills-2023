namespace AtomSkillsTemplate.NewModels
{
    public class ProductForPosition
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public long Quantity { get; set; }
        public long QuantityExec { get; set; }

        public long ProductId { get; set; }

        public string Code { get; set; }
        public string Caption { get; set; }
        public long MillingTime { get; set; }
        public long LatheTime { get; set; }

        

    }
}
