using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

public static class JsonHelper
{
    public static object FromJson<T>(string json)
    {
        if (json.StartsWith("["))
        {
            json = "{\"Items\":" + json + "}";
            var obj = JsonUtility.FromJson<Wrapper<T>>(json);
            return (T[])(object)obj.Items;
        }
        else
        {
            T obj = JsonUtility.FromJson<T>(json);
            return obj;
        }
    }

    public static string ToJson<T>(T obj)
    {
        if (obj is IList)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = obj;
            var json = JsonUtility.ToJson(wrapper);
            json = Regex.Replace(json, "^\\{\"Items\":", "");
            json = Regex.Replace(json, "\\}$", "");
            return json;
        }
        else
        {
            var json = JsonUtility.ToJson(obj);
            return json;
        }
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T Items;
    }
}