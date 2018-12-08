using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OctoPi
{
    public delegate void JobInformationCallback(JobInformationResponse response);

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
            var json = @"{
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
            result = JsonUtility.FromJson<JobInformationResponse>(json);
            callback.Invoke(result);
        }
    }
}