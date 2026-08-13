using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Project
{
    public static class JObjectExt
    {
        public static Vector3 GetVector3(this JToken token, string key, Vector3 def = new())
        {
            return token[key]?.ToObject<Vector3>() ?? def;
        }

        public static void SetVector3(this JToken token, string key, Vector3 value)
        {
            token[key] = JObject.FromObject(value);
        }
        
        public static float GetFloat(this JToken token, string key, float def = 0F)
        {
            return token[key]?.ToObject<float>() ?? def;
        }

        public static void SetFloat(this JToken token, string key, float value)
        {
            token[key] = JObject.FromObject(value);
        }
        
        public static int GetInt(this JToken token, string key, int def = 0)
        {
            return token[key]?.ToObject<int>() ?? def;
        }

        public static void SetInt(this JToken token, string key, int value)
        {
            token[key] = JObject.FromObject(value);
        }
    }
}