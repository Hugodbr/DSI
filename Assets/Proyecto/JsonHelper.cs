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
            ListaWrapper<T> wrapper = JsonUtility.FromJson<ListaWrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(List<T> list)
        {
            ListaWrapper<T> wrapper = new ListaWrapper<T> { Items = list };
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(List<T> list, bool prettyPrint)
        {
            ListaWrapper<T> wrapper = new ListaWrapper<T> { Items = list };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class ListaWrapper<T>
        {
            public List<T> Items;
        }
    }
}