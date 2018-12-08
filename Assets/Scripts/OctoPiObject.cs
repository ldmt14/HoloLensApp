using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OctoPi
{
    public class OctoPiObject : InspectableObject
    {
        [SerializeField]
        private Slider progressBar;
        [SerializeField]
        private Slider temperatureBar;
        [SerializeField]
        private Text fileNameText;

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
                progressBar.value = response.progress.completion * progressBar.maxValue;
                fileNameText.text = response.job.file.name;
            });
        }
    }
}