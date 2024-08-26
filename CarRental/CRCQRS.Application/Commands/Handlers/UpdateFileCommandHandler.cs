using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class UpdateFileCommandHandler : IRequestHandler<UpdateFileCommand, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public UpdateFileCommandHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Validate file
      if (request.File == null || request.File.Length == 0)
      {
        response.Success = false;
        response.Message = "File is invalid";
        response.StatusCode = HttpStatusCode.BadRequest;
        return response;
      }

      var appFile = await _context.AppFiles.FindAsync(new object[] { request.AppFileId }, cancellationToken);
      if (appFile == null)
      {
        response.Success = false;
        response.Message = "File not found";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }

      using (var ms = new MemoryStream())
      {
        await request.File.CopyToAsync(ms, cancellationToken);

        appFile.FileName = request.File.FileName;
        appFile.ContentType = request.File.ContentType;
        appFile.FileSize = request.File.Length;
        appFile.Data = ms.ToArray();
        appFile.UploadedOn = DateTime.Now;

        _context.AppFiles.Update(appFile);
        await _context.SaveChangesAsync(cancellationToken);

        response.Success = true;
        response.Message = "File updated successfully";
        response.StatusCode = HttpStatusCode.OK;
        response.Data = appFile.Id; // Return the ID of the updated file
      }

      return response;
    }
  }
}
