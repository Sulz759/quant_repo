using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace _Project.Architecture.Scripts.Runtime.Utilities.SaveService
{
    public class DataFromStreamingAssetsService: ILoaderService
    {
        private string _path;
        public void Load<T>(string key, Action<T> callback)
        {
            var path = BuildPathToLoad(key);

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

        public string BuildPathToLoad(string key)
        {
            var loadJson = Path.Combine(Application.streamingAssetsPath + $"/{key}.json");
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _path = loadJson;
                    return loadJson;
                case RuntimePlatform.WindowsEditor:
                    Coroutines.StartRoutine(BuildPathOnAndroidRoutine(loadJson));
                    Debug.Log("step 2" + _path);
                    return _path;
                    
                default: return null;
            }
            
        }

        private IEnumerator BuildPathOnAndroidRoutine(string json)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(json))
            {
                _path = webRequest.url;
                yield return webRequest.SendWebRequest();
                Debug.Log(_path);
            }
        }

        /*private IEnumerator BuildPathOnAndroidRoutine(string json)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(json))
            {
                //Fetches a page and displays the number of characters of the response.
                yield return request.SendWebRequest();
                json = request.result.ToString();
            }
        }*/
    }
}