namespace CRCQRS.Application.DTO
{
  public class BookingCarDto
  {
    public int Id { get; set; }
    public int CarId { get; set; }
    public long CustomerId { get; set; }
    public string Address { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CarName { get; set; }
  }
}
