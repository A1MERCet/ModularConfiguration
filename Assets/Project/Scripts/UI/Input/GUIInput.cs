using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIInput : MonoBehaviour
{
    public LoggerProxy logger = new LoggerProxy("WMF配置输入框");
    
    public enum Type
    {
        BOOLEAN,
        INTEGER,
        FLOAT,
        STRING,
        VECTOR3,
        VECTOR2,
    }
    
    public bool isType;
    public bool isRender;
    public string propertyPath;
    
    public Type type = Type.FLOAT;
    public Toggle toggle;
    public InputField input1;
    public InputField input2;
    public InputField input3;
    public UnityEvent<GUIInput, object> onValueChanged = new();

    public void SetValue(object v)
    {
        if (type == Type.BOOLEAN && v is bool b) {
            toggle.isOn = b;
        }else if (type == Type.VECTOR2) {
            Vector2 vec2 = ParseVector2(v);
            input1.text = $"{vec2.x:F}";
            input2.text = $"{vec2.y:F}";
        }else if (type == Type.VECTOR3) {
            Vector3 vec3 = ParseVector3(v);
            input1.text = $"{vec3.x:F}";
            input2.text = $"{vec3.y:F}";
            input3.text = $"{vec3.z:F}";
        }else if (type == Type.INTEGER) {
            input1.text = v == null ? $"0" : v.ToString();
        }else if (type == Type.FLOAT) {
            input1.text = v == null ? $"0.0" : $"{v:F}";
        }else if (type == Type.STRING) {
            input1.text = v == null ? "" : v.ToString();
        }else {
            logger.Error($"设置 {gameObject.name} - {v}({v?.GetType().Name ?? "null"}) 失败");
        }
    }

    public object GetValue()
    {
        switch (type)
        {
            case Type.INTEGER: {
                int.TryParse(input1.text, out int value);
                return value;
            } case Type.FLOAT: {
                float.TryParse(input1.text, out float value);
                return value;
            } case Type.VECTOR2: {
                float.TryParse(input1.text, out float x);
                float.TryParse(input2.text, out float y);
                return new Vector2(x, y);
            } case Type.VECTOR3: {
                float.TryParse(input1.text, out float x);
                float.TryParse(input2.text, out float y);
                float.TryParse(input3.text, out float z);
                return new Vector3(x, y, z);
            } case Type.BOOLEAN: {
                return toggle.isOn;
            }
            case Type.STRING: {
                return input1.text; 
            }
        }
        return input1.text;
    }

    private Vector3 ParseVector3(object v)
    {
        if (v is Vector3 vec3) return vec3;
        if (v is JObject jo) return new Vector3(jo["x"]?.Value<float>() ?? 0F, jo["y"]?.Value<float>() ?? 0F, jo["z"]?.Value<float>() ?? 0F);
        if (v is string s) {
            var parts = s.Trim('(', ')').Split(',');
            return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
        }
        return Vector3.zero;
    }

    private Vector2 ParseVector2(object v)
    {
        if (v is Vector2 vec2) return vec2;
        if (v is JObject jo) return new Vector2(jo["x"]?.Value<float>() ?? 0F, jo["y"]?.Value<float>() ?? 0F);
        if (v is string s) {
            var parts = s.Trim('(', ')').Split(',');
            return new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
        }
        return Vector2.zero;
    }
    
    private void OnValueChanged(object v)
    {
        onValueChanged?.Invoke(this, GetValue());
    }

    public void ActionToggle(bool v)
    {
        OnValueChanged(v);
    }
    
    public void ActionValue1Changed(string v)
    {
        OnValueChanged(v);
    }
    
    public void ActionValue2Changed(string v)
    {
        OnValueChanged(v);
    }
    
    public void ActionValue3Changed(string v)
    {
        OnValueChanged(v);
    }
}
