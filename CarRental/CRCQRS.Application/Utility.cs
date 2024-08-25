using Newtonsoft.Json;
namespace CRCQRS.Application
{

  public static class Utility
  {
    /// <summary>
    /// Converts any object to a JSON string.
    /// </summary>
    /// <param name="obj">The object to convert.</param>
    /// <returns>A JSON string representation of the object.</returns>
    public static string ConvertObjectToJsonString(object obj)
    {
      try
      {
        var settings = new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        return JsonConvert.SerializeObject(obj, settings);
      }
      catch (Exception ex)
      {
        // Handle or log exception as needed
        throw new InvalidOperationException("Failed to convert object to JSON string.", ex);
      }
    }


    /// <summary>
    /// Converts a JSON string back into an object of a specified type.
    /// </summary>
    /// <param name="jsonString">The JSON string to convert.</param>
    /// <param name="typeName">The full name of the type to convert the JSON string into.</param>
    /// <returns>An object of the specified type.</returns>
    public static object ConvertJsonStringToObject(string jsonString, string typeName)
    {
      try
      {
        // Get the type from the type name
        Type objectType = Type.GetType(typeName);
        if (objectType == null)
        {
          throw new ArgumentException($"Type '{typeName}' could not be found.");
        }

        // Deserialize the JSON string to the specified type
        return JsonConvert.DeserializeObject(jsonString, objectType);
      }
      catch (Exception ex)
      {
        // Handle or log exception as needed
        throw new InvalidOperationException($"Failed to convert JSON string to object of type '{typeName}'.", ex);
      }
    }
  }

}
