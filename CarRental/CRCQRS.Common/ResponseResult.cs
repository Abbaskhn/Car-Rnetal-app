using System.Net;

namespace CRCQRS.Common
{
  public class ResponseResult
  {
    public bool Success { get; set; }
    public bool IsSuccess { get { return Success; } }
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public object Data { get; set; } // Contains the response detail data
    public long RecordsEffected { get; set; }
    public long TotalRecords { get; set; }

    public ResponseResult()
    {
      Success = false;
      Message = string.Empty;
      StatusCode = 0;
      Data = null;
      RecordsEffected = 0;
      TotalRecords = 0;
    }
  }
}
