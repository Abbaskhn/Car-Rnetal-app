using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class RegisterCarCommandHandler : IRequestHandler<RegisterCarCommand, ResponseResult>
  {
    private readonly IMediator _mediator;
    private readonly CRCQRSContext _context;
    private readonly IUserInfoService _userSrv;


    public RegisterCarCommandHandler(CRCQRSContext context, IUserInfoService userSrv, IMediator mediator)
    {
      _context = context;
      _mediator = mediator;
      _userSrv = userSrv;

    }

    public async Task<ResponseResult> Handle(RegisterCarCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      Car objCar = new Car() { CarName = request.CarName, Model = request.Model, Rentalprice = request.Rentalprice };

      _context.Cars.Add(objCar);
      CarFile objCarFile = new CarFile() { AppFileId = request.FileId, Car = objCar };
      _context.CarFiles.Add(objCarFile);
      await _context.SaveChangesAsync(cancellationToken);
      UserInfo userInfo = await _userSrv.GetUserInfo();
      response.Success = true;
      response.Message = "Car Added successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = objCar;

      string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) registered a car on: {DateTime.Now}";
      await _mediator.Publish(new LoggingEvent("Information", statement, DateTime.UtcNow, userInfo.UserID, objCar));

      return response;
    }


  }
}
