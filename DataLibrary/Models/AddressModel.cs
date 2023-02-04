namespace DataLibrary.Models
{
    public class AddressModel
    {
        public string? AccountId { get; set; }
        public string? AccountNumber { get; set; }
        public string? Ward { get; set; }
        public string? Section { get; set; }
        public string? Block { get; set; }
        public string? Lot { get; set; }
        public string? LandUseCode { get; set; }
        public int? YearBuilt { get; set; }
        public bool? IsGroundRent { get; set; }
        public bool? IsRedeemed { get; set; }
        public int? PdfCount { get; set; }
        public bool? AllPdfsDownloaded { get; set; }
    }
}
