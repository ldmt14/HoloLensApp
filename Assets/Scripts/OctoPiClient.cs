#define LDMT_TESTING

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace OctoPi
{
    public delegate void JobInformationCallback(bool success, JobInformationResponse response);
    public delegate void StateInformationCallback(bool success, FullStateResponse response);
    public delegate void FileInformationCallback(bool success, FileInformation response);

    public class OctoPiClient : MonoBehaviour
    {
        private static OctoPiClient Instance;
        public static readonly string domain = "http://10.10.10.13";
        public static readonly string xApiKey = "494F97703CD14F529D919058C1D2360E";
        // Use this for initialization
        void Start()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void GetAndStoreFile(string download, string storagePath, Action<bool> callback)
        {
            Instance.StartCoroutine(GetAndStoreFileInternal(download, storagePath, callback));
        }

        private static IEnumerator GetAndStoreFileInternal(string download, string storagePath, Action<bool> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(download);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                callback.Invoke(false);
            } else
            {
                System.IO.File.WriteAllBytes(storagePath, request.downloadHandler.data);
                callback.Invoke(true);
            }
            /*
            client.Get(new Uri(download), HttpCompletionOption.AllResponseContent, (response) =>
            {
                System.IO.File.WriteAllBytes(storagePath, response.ReadAsByteArray());
            });
            */
        }

        public static void GetFileInformation(string location, string path, FileInformationCallback callback)
        {
            Instance.StartCoroutine(GetFileInformationInternal(location, path, callback));
        }

        private static IEnumerator GetFileInformationInternal(string location, string path, FileInformationCallback callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(domain + "/api/files/" + location + "/" + path);
            request.SetRequestHeader("x-api-key", xApiKey);
            yield return request.SendWebRequest();
            if (request.isHttpError)
            {
                callback.Invoke(false, null);
            } else
            {
                var json = System.Text.Encoding.ASCII.GetString(request.downloadHandler.data);
                Debug.Log("File Information recieved: " + json);
                FileInformation result = JsonConvert.DeserializeObject<FileInformation>(json);
                callback.Invoke(true, result);
            }
            /*
            client.Get(new Uri(domain + "/api/file/" + location + "/" + path), HttpCompletionOption.AllResponseContent, (response) =>
            {
                if (response.IsSuccessStatusCode)
                {
                    FileInformation result;
                    var json = response.ReadAsString();
                    result = JsonConvert.DeserializeObject<FileInformation>(json);
                    callback.Invoke(true, result);
                } else
                {
                    callback.Invoke(false, null);
                }
            });
            */
        }

        public static void GetJobInformation(JobInformationCallback callback)
        {
            Instance.StartCoroutine(GetJobInformationInternal(callback));
        }

        private static IEnumerator GetJobInformationInternal(JobInformationCallback callback)
        {
            JobInformationResponse result;
#if LDMT_TESTING
            yield return null;
            var json = @"
{
  ""job"": {
    ""file"": {
      ""name"": ""20mm_cube.gco"",
      ""origin"": ""local"",
      ""size"": 1468987,
      ""date"": 1378847754
    },
    ""estimatedPrintTime"": 8811,
    ""filament"": {
      ""length"": 810,
      ""volume"": 5.36
    }
  },
  ""progress"": {
    ""completion"": 0.2298468264184775,
    ""filepos"": 337942,
    ""printTime"": 276,
    ""printTimeLeft"": 912
  }
}";
#else
            UnityWebRequest request = UnityWebRequest.Get(domain + "/api/job");
            request.SetRequestHeader("x-api-key", xApiKey);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                callback.Invoke(false, null);
            } else
            {
                var json = System.Text.Encoding.ASCII.GetString(request.downloadHandler.data);
#endif
                result = JsonConvert.DeserializeObject<JobInformationResponse>(json);
                callback.Invoke(true, result);
#if !LDMT_TESTING
            }
#endif
        }

        public static void GetStateInformation(StateInformationCallback callback)
        {
            Instance.StartCoroutine(GetStateInformationInternal(callback));
        }

        private static IEnumerator GetStateInformationInternal(StateInformationCallback callback)
        {
            FullStateResponse result;
#if LDMT_TESTING
            yield return null;
            var json = @"
{
  ""temperature"": {
    ""tool0"": {
                ""actual"": 214.8821,
      ""target"": 220.0,
      ""offset"": 0
    },
    ""tool1"": {
                ""actual"": 25.3,
      ""offset"": 0
    },
    ""bed"": {
                ""actual"": 50.221,
      ""target"": 70.0,
      ""offset"": 5
    },
    ""history"": [
      {
        ""time"": 1395651928,
        ""tool0"": {
          ""actual"": 214.8821,
          ""target"": 220.0
        },
        ""tool1"": {
          ""actual"": 25.3,
        },
        ""bed"": {
          ""actual"": 50.221,
          ""target"": 70.0
        }
      },
      {
        ""time"": 1395651926,
        ""tool0"": {
          ""actual"": 212.32,
          ""target"": 220.0
        },
        ""tool1"": {
          ""actual"": 25.1,
        },
        ""bed"": {
          ""actual"": 49.1123,
          ""target"": 70.0
        }
      }
    ]
  },
  ""sd"": {
    ""ready"": true
  },
  ""state"": {
    ""text"": ""Operational"",
    ""flags"": {
      ""operational"": true,
      ""paused"": false,
      ""printing"": false,
      ""cancelling"": false,
      ""pausing"": false,
      ""sdReady"": true,
      ""error"": false,
      ""ready"": true,
      ""closedOrError"": false
    }
  }
}";
#else
            UnityWebRequest request = UnityWebRequest.Get(domain + "/api/printer");
            request.SetRequestHeader("x-api-key", xApiKey);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                callback.Invoke(false, null);
            } else
            {
                var json = System.Text.Encoding.ASCII.GetString(request.downloadHandler.data);
#endif
                result = JsonConvert.DeserializeObject<FullStateResponse>(json);
                callback.Invoke(true, result);
#if !LDMT_TESTING
            }
#endif
            /*
        client.Get(new Uri(domain + ""), HttpCompletionOption.AllResponseContent, (response) =>
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Debug.Log("Unsuccessfull Request");
                return;
            }
            var json = response.ReadAsString();
#endif
            result = JsonConvert.DeserializeObject<FullStateResponse>(json);
            callback.Invoke(result);
*/
        }
    }
}