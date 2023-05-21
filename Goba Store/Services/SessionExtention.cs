using System.Text.Json;

namespace Goba_Store.Services
{
    public static class SessionExtention
    {
        public static void SetObj<T>(this ISession session,string key , T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObj<T>(this ISession session,string key)
        {
            var serializedValue = session.GetString(key);
            return serializedValue == null ? default : JsonSerializer.Deserialize<T>(serializedValue);
        }
    
    }
}
