using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

/// <summary>
/// This TempData class basically store for object into an object format and I want to store the values in the JSON format.
/// You can use something another way, we just created a common method for this so we can use this method whenever we want to store the value into an temp data, and we can also avoid the code duplication.
/// </summary>
namespace ePizzaHub.UI.Helpers
{
    public static class TempDataExtension
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            tempData[key] = JsonSerializer.Serialize(value, options);
        }

        public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o = tempData.Peek(key);
            return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData.TryGetValue(key, out var obj);
            return obj == null ? null : JsonSerializer.Deserialize<T>((string)obj);
        }
    }
}
