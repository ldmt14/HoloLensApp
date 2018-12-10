//#define LDMT_TESTING

using CI.HttpClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OctoPi
{
    public delegate void JobInformationCallback(JobInformationResponse response);
    public delegate void StateInformationCallback(FullStateResponse response);

    public class OctoPiClient : MonoBehaviour
    {
        public static HttpClient client;
        private static readonly string domain = "http://localhost";
        // Use this for initialization
        void Start()
        {
            client = new HttpClient();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void GetJobInformation(JobInformationCallback callback)
        {
            JobInformationResponse result;
#if LDMT_TESTING
            var json = @"
{
  ""job"": {
    ""file"": {
      ""name"": ""whistle_v2.gcode"",
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
            client.Get(new Uri(domain + "/api/job"), HttpCompletionOption.AllResponseContent, (response) =>
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Debug.Log("Unsuccessfull Request");
                    return;
                }
                var json = response.ReadAsString();
#endif
                result = JsonConvert.DeserializeObject<JobInformationResponse>(json);
                callback.Invoke(result);
#if !LDMT_TESTING
            });
#endif
        }

        public static void GetStateInformation(StateInformationCallback callback)
        {
            FullStateResponse result;
#if LDMT_TESTING
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
#if !LDMT_TESTING
            });
#endif
        }
    }
}