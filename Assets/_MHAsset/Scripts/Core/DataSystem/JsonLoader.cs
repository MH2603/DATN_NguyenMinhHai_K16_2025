
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MH.DataSystem
{
    public class JsonLoader<T> : IDataLoader<T> where T : IIdentifiable
    {
        [Serializable]
        private class Wrapper { public List<T> items; }

        public bool Exists(string path)
        {
            throw new System.NotImplementedException();
        }

        public List<T> Load(string fileName)
        {

            string relativePath = "_MHAsset/Json/" + fileName;
            string fullPath = Path.Combine(Application.dataPath, relativePath);

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"File not found at path: {fullPath}");
                return new List<T>();
            }

            string rawJsonContent = File.ReadAllText(fullPath);
            string jsonContent = "{\"items\":" + rawJsonContent + "}";

            return JsonUtility.FromJson<Wrapper>(jsonContent).items;
        }
    }
}
