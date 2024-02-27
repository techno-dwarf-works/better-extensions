using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Extensions.Runtime
{
    public static class AssetDatabaseUtility
    {
        private const string PrefabFilter = "t:prefab";
        private const string ScriptableObjectFilter = "t:ScriptableObject";

        public static T[] FindPrefabsOfType<T>()
        {
            var prefabs = new List<T>();
            var gameObjects = FindAssetsOfType<GameObject>(PrefabFilter);

            foreach (var gameObject in gameObjects)
            {
                var components = gameObject.GetComponents<Component>();
                foreach (var component in components)
                {
                    if (component is not T item) continue;

                    prefabs.Add(item);
                    break;
                }
            }

            return prefabs.ToArray();
        }

        public static T[] FindScriptableObjectsOfType<T>() where T : ScriptableObject
        {
            return FindAssetsOfType<T>(ScriptableObjectFilter);
        }

        public static T[] FindAssetsOfType<T>(string filter) where T : Object
        {
            if (filter.IsNullOrEmpty() || filter.IsNullOrWhiteSpace())
            {
                var message = $"{nameof(filter)} cannot be Null or Empty";
                DebugUtility.LogException<ArgumentException>(message);
                return Array.Empty<T>();
            }

            var assets = new List<T>();
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets(filter);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);

                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

#endif

            return assets.ToArray();
        }
    }
}