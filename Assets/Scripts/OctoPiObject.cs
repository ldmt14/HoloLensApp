using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OctoPi
{
    public class OctoPiObject : InspectableObject
    {
        public OctoPiInfoObject octoPiInfo;

        public override void Help()
        {
            throw new System.NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {
            InvokeRepeating("UpdateUI", 2, 2);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateUI()
        {
            OctoPiClient.GetJobInformation((response) =>
            {
                octoPiInfo.ProgressBar.value = response.progress.completion * octoPiInfo.ProgressBar.maxValue;
                octoPiInfo.FileNameText.text = response.job.file.name;
            });
            OctoPiClient.GetStateInformation((response) =>
            {
                octoPiInfo.TemperatureBar.TemperatureData = response.temperature.tool0;
            });
        }
    }
}