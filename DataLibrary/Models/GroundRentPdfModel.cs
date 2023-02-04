namespace DataLibrary.Models;

public class GroundRentPdfModel
{
    public int Id { get; set; }
    public string? AccountId { get; set; }
    public string? DocumentFiledType { get; set; }
    public string? AcknowledgementNumber { get; set; }
    public DateTime? DateTimeFiled { get; set; }
    public string? DateTimeFiledString { get; set; }
    public string? PageAmount { get; set; }
    public string? Book { get; set; }
    public string? Page { get; set; }
    public string? ClerkInitials { get; set; }
    public int? YearRecorded { get; set; }
}
