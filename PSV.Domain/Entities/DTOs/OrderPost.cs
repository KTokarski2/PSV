namespace PSV.Domain.Entities.DTOs
{
    public class OrderPost
    {
        public int IdClient { get; set; }
        public string Client { get; set; }
        public string QrCode { get; set; }
        public string Format { get; set; }
        public string Comments { get; set; }
        public string Photos { get; set; }
        public bool IsCutPresent { get; set; }
        public bool IsMillingPresent { get; set; }
        public bool IsWrappingPresent{ get; set; }
    }
}