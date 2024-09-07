namespace CRCQRS.Common
{
  public static class Constants
  {
    public static class Messages
    {
      public const string CAR_ADDED_SUCCESSFULLY = "Car Added successfully";

      public const string BOOKING_NOT_FOUND = "Booking not found";
      public const string BOOKING_ADDED_SUCCESSFULLY = "Booking deleted successfully";
      public const string USER_AUTHORIZATION = "User is not authorized.";

      public const string VENDOR_DELETED_MESSAGE = "Vendor deleted successfully";
      public const string VENDOR_NOTFOUND_MESSAGE = "Vendor deletion failed: Vendor not found";

      public const string CUSTOMER_DELETED_MESSAGE = "Customer deleted successfully";
      public const string CUSTOMER_NOT_MESSAGE = "Customer deletion failed: Vendor not found";

      public const string CAR_NOTFOUND_MSG = "Car not found.";
      public const string CAR_ADDED_MSG = "Car Delete successfully";
      public const string LOGIN_MESSAGE  = "Login successful";
      public const string INVALID_CREDENTIAL_MEASSAGE = "Invalid credentials";

    }
    public static class LogLevels
    {
      public const string INFORMATION = "Information";
      public const string WARNING = "Warning";
      public const string ERROR = "Error";
    }
    public static class Authorization
    {
      public const string AUTHORIZATION_HEADER = "Authorization";
      public const string TOKEN_TYPE = "Bearer ";
    }
   
  }
}
