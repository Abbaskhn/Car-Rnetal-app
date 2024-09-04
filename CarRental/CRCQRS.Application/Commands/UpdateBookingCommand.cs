using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class UpdateBookingCommand : IRequest<ResponseResult>
  {  public int Id { get; set; }
    public int CarId { get; set; }
    public long CustomerId { get; set; }
    public string Address { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalAmount { get; set; }
  }
}
