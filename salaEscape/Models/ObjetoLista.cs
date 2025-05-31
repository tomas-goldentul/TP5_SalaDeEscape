using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;

namespace salaEscape.Models;

public static class ObjetoLista
{
    public static void GuardarLista<T>(ISession session, string key, List<T> lista)
    {
        var json = JsonSerializer.Serialize(lista);
        session.SetString(key, json);
    }

    public static List<T> ObtenerLista<T>(ISession session, string key)
    {
        var json = session.GetString(key);
        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }
        return JsonSerializer.Deserialize<List<T>>(json);
    }
}
