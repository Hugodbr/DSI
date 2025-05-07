using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProyectoMain
{
    public static class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(List<T> list)
        {
            ListWrapper<T> wrapper = new ListWrapper<T> { Items = list };
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(List<T> list, bool prettyPrint)
        {
            ListWrapper<T> wrapper = new ListWrapper<T> { Items = list };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class ListWrapper<T>
        {
            public List<T> Items;
        }
    }
}