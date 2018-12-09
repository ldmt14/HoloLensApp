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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void GetJobInformation(JobInformationCallback callback)
        {
            JobInformationResponse result;
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
            result = JsonConvert.DeserializeObject<JobInformationResponse>(json);
            callback.Invoke(result);
        }

        public static void GetStateInformation(StateInformationCallback callback)
        {
            FullStateResponse result;
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
            result = JsonConvert.DeserializeObject<FullStateResponse>(json);
            callback.Invoke(result);
        }
    }
}