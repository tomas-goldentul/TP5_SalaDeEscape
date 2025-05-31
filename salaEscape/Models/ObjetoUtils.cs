using Newtonsoft.Json;

namespace salaEscape.Models;

public static class ObjetoUtils
{
    public static string ObjectToString<T>(T? obj)
    {
        return obj != null ? JsonConvert.SerializeObject(obj) : string.Empty;
    }

    public static T? StringToObject<T>(string? txt)
    {
        if (string.IsNullOrEmpty(txt))
            return default;
        try
        {
            return JsonConvert.DeserializeObject<T>(txt);
        }
        catch
        {
            return default;
        }
    }
}