using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public UploadFileCommandHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      if (request.File == null || request.File.Length == 0)
      {
        response.Success = false;
        response.Message = "File is invalid";
        response.StatusCode = HttpStatusCode.BadRequest;
        return response;
      }

      using (var ms = new MemoryStream())
      {
        await request.File.CopyToAsync(ms, cancellationToken);

        var appFile = new AppFile
        {
          FileName = request.File.FileName,
          ContentType = request.File.ContentType,
          FileSize = request.File.Length,
          Data = ms.ToArray(),
          UploadedOn = DateTime.Now,
        };

        _context.AppFiles.Add(appFile);
        await _context.SaveChangesAsync(cancellationToken);

        response.Success = true;
        response.Message = "File uploaded successfully";
        response.StatusCode = HttpStatusCode.OK;
        response.Data = new { FileId = appFile.Id };

        return response;
      }
    }
  }
}
