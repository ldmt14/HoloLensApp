#define LDMT_TESTING_WITHOUT_PRINT_JOB
#define LDMT_TESTING_WITHOUT_OCTOPI

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace OctoPi
{
    public delegate void JobInformationCallback(bool success, JobInformationResponse response);
    public delegate void StateInformationCallback(bool success, FullStateResponse response);
    public delegate void FileInformationCallback(bool success, FileInformation response);
    public delegate void FileDownloadCallback(bool success, byte[] FileText);

    public class OctoPiClient : MonoBehaviour
    {
        private static OctoPiClient Instance;
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

        public static void GetFile(string download, FileDownloadCallback callback)
        {
            Instance.StartCoroutine(GetFileInternal(download, callback));
        }

        private static IEnumerator GetFileInternal(string download, FileDownloadCallback callback)
        {
#if LDMT_TESTING_WITHOUT_OCTOPI
            var data = Encoding.UTF8.GetBytes(@"solid model
facet normal 0.0 0.0 -1.0
outer loop
vertex 40.0 0.0 0.0
vertex 0.0 -40.0 0.0
vertex 0.0 0.0 0.0
endloop
endfacet
facet normal 0.0 0.0 -1.0
outer loop
vertex 0.0 -40.0 0.0
vertex 40.0 0.0 0.0
vertex 40.0 -40.0 0.0
endloop
endfacet
facet normal -0.0 -1.0 -0.0
outer loop
vertex 40.0 -40.0 40.0
vertex 0.0 -40.0 0.0
vertex 40.0 -40.0 0.0
endloop
endfacet
facet normal -0.0 -1.0 -0.0
outer loop
vertex 0.0 -40.0 0.0
vertex 40.0 -40.0 40.0
vertex 0.0 -40.0 40.0
endloop
endfacet
facet normal 1.0 0.0 0.0
outer loop
vertex 40.0 0.0 0.0
vertex 40.0 -40.0 40.0
vertex 40.0 -40.0 0.0
endloop
endfacet
facet normal 1.0 0.0 0.0
outer loop
vertex 40.0 -40.0 40.0
vertex 40.0 0.0 0.0
vertex 40.0 0.0 40.0
endloop
endfacet
facet normal -0.0 -0.0 1.0
outer loop
vertex 40.0 -40.0 40.0
vertex 0.0 0.0 40.0
vertex 0.0 -40.0 40.0
endloop
endfacet
facet normal -0.0 -0.0 1.0
outer loop
vertex 0.0 0.0 40.0
vertex 40.0 -40.0 40.0
vertex 40.0 0.0 40.0
endloop
endfacet
facet normal -1.0 0.0 0.0
outer loop
vertex 0.0 0.0 40.0
vertex 0.0 -40.0 0.0
vertex 0.0 -40.0 40.0
endloop
endfacet
facet normal -1.0 0.0 0.0
outer loop
vertex 0.0 -40.0 0.0
vertex 0.0 0.0 40.0
vertex 0.0 0.0 0.0
endloop
endfacet
facet normal -0.0 1.0 0.0
outer loop
vertex 0.0 0.0 40.0
vertex 40.0 0.0 0.0
vertex 0.0 0.0 0.0
endloop
endfacet
facet normal -0.0 1.0 0.0
outer loop
vertex 40.0 0.0 0.0
vertex 0.0 0.0 40.0
vertex 40.0 0.0 40.0
endloop
endfacet
endsolid model
");
            yield return null;
#else
            UnityWebRequest request = UnityWebRequest.Get(download);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                callback.Invoke(false, null);
            } else
            {
                var data = request.downloadHandler.data;
#endif
                callback.Invoke(true, data);
#if !LDMT_TESTING_WITHOUT_OCTOPI
            }
#endif
        }

        public static void GetFileInformation(string domain, string location, string path, FileInformationCallback callback)
        {
            Instance.StartCoroutine(GetFileInformationInternal(domain, location, path, callback));
        }

        private static IEnumerator GetFileInformationInternal(string domain, string location, string path, FileInformationCallback callback)
        {
#if LDMT_TESTING_WITHOUT_OCTOPI
            var json = @"
{
  ""name"": ""BVS.gco"",
  ""size"": 1468987,
  ""date"": 1378847754,
  ""origin"": ""local"",
  ""refs"": {
    ""resource"": ""http://example.com/api/files/local/BVS.gco"",
    ""download"": ""http://example.com/downloads/files/local/BVS.gco""
  },
  ""gcodeAnalysis"": {
    ""estimatedPrintTime"": 1188,
    ""filament"": {
      ""length"": 810,
      ""volume"": 5.36
    }
  },
  ""print"": {
    ""failure"": 4,
    ""success"": 23,
    ""last"": {
      ""date"": 1387144346,
      ""success"": true
    }
  }
}";
            yield return null;
#else
            UnityWebRequest request = UnityWebRequest.Get(domain + "/api/files/" + location + "/" + path);
            request.SetRequestHeader("x-api-key", xApiKey);
            yield return request.SendWebRequest();
            if (request.isHttpError)
            {
                callback.Invoke(false, null);
            } else
            {
                var json = System.Text.Encoding.ASCII.GetString(request.downloadHandler.data);
#endif
                FileInformation result = JsonConvert.DeserializeObject<FileInformation>(json);
                callback.Invoke(true, result);
#if !LDMT_TESTING_WITHOUT_OCTOPI
            }
#endif
        }

        public static void GetJobInformation(string domain, JobInformationCallback callback)
        {
            Instance.StartCoroutine(GetJobInformationInternal(domain, callback));
        }

        private static IEnumerator GetJobInformationInternal(string domain, JobInformationCallback callback)
        {
            JobInformationResponse result;
#if LDMT_TESTING_WITHOUT_PRINT_JOB
            yield return null;
            var json = @"
{
  ""job"": {
    ""file"": {
      ""name"": ""BVS.gco"",
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
#if !LDMT_TESTING_WITHOUT_PRINT_JOB
            }
#endif
        }

        public static void GetStateInformation(string domain, StateInformationCallback callback)
        {
            Instance.StartCoroutine(GetStateInformationInternal(domain, callback));
        }

        private static IEnumerator GetStateInformationInternal(string domain, StateInformationCallback callback)
        {
            FullStateResponse result;
#if LDMT_TESTING_WITHOUT_PRINT_JOB
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
#if !LDMT_TESTING_WITHOUT_PRINT_JOB
            }
#endif
        }
    }
}