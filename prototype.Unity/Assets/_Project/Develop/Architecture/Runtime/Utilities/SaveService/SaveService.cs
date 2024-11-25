using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Utilities.SaveService
{
    public class SaveService: IStorageService
    {
        private bool _isInProgressNow;
        
        public void Save(string key, object data, Action<bool> callback = null)
        {
            if (!_isInProgressNow)
            {
                SaveAsync(key,data, callback);
            }
            else
            {
                callback?.Invoke(false);
            }
        }

        public void Load<T>(string key, Action<T> callback)
        {
            var path = BuildPath(key);

            try
            {
                using (var fileStream = new StreamReader(path))
                {
                    var json = fileStream.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<T>(json);

                    callback.Invoke(data);
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e);
            }
        }

        private async void SaveAsync(string key, object data, Action<bool> callback)
        {
            var path = BuildPath(key);
            var json = JsonConvert.SerializeObject(data);

            await using (var fileStream = new StreamWriter(path))
            {
               await fileStream.WriteAsync(json);
            }
            callback?.Invoke(true);
        }

        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath + $"/{key}.json");
        }
    }
}