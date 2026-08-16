using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Project
{
    public static class JObjectExt
    {
        private static JToken GetTokenByPath(this JToken root, string path)
        {
            if (root == null || string.IsNullOrEmpty(path)) return root;

            string[] parts = path.Split('.');
            JToken current = root;

            foreach (var part in parts)
            {
                if (current is JObject obj)
                    current = obj[part];
                else
                    return null;

                if (current == null) return null;
            }
            return current;
        }
        private static JObject EnsureObjectByPath(this JToken root, string path)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (string.IsNullOrEmpty(path))
            {
                if (root is JObject jo) return jo;
                throw new InvalidOperationException("根节点不是 JObject");
            }

            string[] parts = path.Split('.');
            JToken current = root;

            foreach (var part in parts)
            {
                if (current is JObject obj)
                {
                    var next = obj[part];
                    if (next == null || !(next is JObject))
                    {
                        next = new JObject();
                        obj[part] = next;
                    }
                    current = next;
                }
                else
                {
                    throw new InvalidOperationException($"路径中间节点不是 JObject，无法创建: {part}");
                }
            }
            return current as JObject;
        }
        public static T Get<T>(this JToken token, string path, T def = default)
        {
            var target = token.GetTokenByPath(path);
            if (target == null) return def;

            try { return target.ToObject<T>(); }
            catch { return def; }
        }
        public static object Get(this JToken token, string path, object def = null)
        {
            var target = token.GetTokenByPath(path);
            if (target == null) return def;
            try { return target.ToObject<object>(); }
            catch { return def; }
        }
        public static void Set<T>(this JToken token, string path, T value)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("路径不能为空", nameof(path));

            string[] parts = path.Split('.');

            // 对某些类型特殊处理 避免循环引用(Vector3.Normalize())
            JToken jValue;
            switch (value)
            {
                case Vector3 v3: jValue = new JObject { ["x"] = v3.x, ["y"] = v3.y, ["z"] = v3.z }; break;
                case Vector2 v2: jValue = new JObject { ["x"] = v2.x, ["y"] = v2.y }; break;
                case Vector4 v4: jValue = new JObject { ["x"] = v4.x, ["y"] = v4.y, ["z"] = v4.z, ["w"] = v4.w }; break;
                case Quaternion q: jValue = new JObject { ["x"] = q.x, ["y"] = q.y, ["z"] = q.z, ["w"] = q.w }; break;
                case Color c: jValue = new JObject { ["r"] = c.r, ["g"] = c.g, ["b"] = c.b, ["a"] = c.a }; break;
                default: jValue = JToken.FromObject(value); break;
            }

            if (parts.Length == 1) {
                if (token is JObject obj) {
                    obj[path] = jValue;
                    return;
                }
                throw new InvalidOperationException("只能在 JObject 上设置值");
            }

            string parentPath = string.Join(".", parts, 0, parts.Length - 1);
            string key = parts[parts.Length - 1];

            var parent = token.EnsureObjectByPath(parentPath);
            parent[key] = jValue;
        }
        // public static void Set<T>(this JToken token, string path, T value)
        // {
        //     if (token == null) throw new ArgumentNullException(nameof(token));
        //     if (string.IsNullOrEmpty(path)) throw new ArgumentException("路径不能为空", nameof(path));
        //
        //     string[] parts = path.Split('.');
        //
        //     if (parts.Length == 1)
        //     {
        //         if (token is JObject obj)
        //         {
        //             obj[path] = JToken.FromObject(value);
        //             return;
        //         }
        //         throw new InvalidOperationException("只能在 JObject 上设置值");
        //     }
        //
        //     string parentPath = string.Join(".", parts, 0, parts.Length - 1);
        //     string key = parts[parts.Length - 1];
        //
        //     var parent = token.EnsureObjectByPath(parentPath);
        //     parent[key] = JToken.FromObject(value);
        // }

        public static Vector3 GetVector3(this JToken token, string path, Vector3 def = default) => token.Get(path, def);
        public static void SetVector3(this JToken token, string path, Vector3 value) => token.Set(path, value);
        public static float GetFloat(this JToken token, string path, float def = 0f) => token.Get(path, def);
        public static void SetFloat(this JToken token, string path, float value) => token.Set(path, value);
        public static int GetInt(this JToken token, string path, int def = 0) => token.Get(path, def);
        public static void SetInt(this JToken token, string path, int value) => token.Set(path, value);
        public static string GetString(this JToken token, string path, string def = null) => token.Get(path, def);
        public static void SetString(this JToken token, string path, string value) => token.Set(path, value);
        public static bool GetBool(this JToken token, string path, bool def = false) => token.Get(path, def);
        public static void SetBool(this JToken token, string path, bool value) => token.Set(path, value);
    }
}