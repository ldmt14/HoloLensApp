using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OctoPi;
using System;
using UnityEngine.UI;

public class TemperatureUI : MonoBehaviour {
    [SerializeField]
    private Slider temperatureSlider;
    [SerializeField]
    private Text actualTemperatureText;
    [SerializeField]
    private GameObject targetTemperatureObject;
    [SerializeField]
    private Text targetTemperatureText;
    private TemperatureData temperataureData;
    public TemperatureData TemperatureData
    {
        private get
        {
            return temperataureData;
        }
        set
        {
            temperataureData = value;
            UpdateTemperatureUI();
        }
    }

    private void UpdateTemperatureUI()
    {
        temperatureSlider.value = TemperatureData.actual;
        actualTemperatureText.text = TemperatureData.actual.ToString("0.0") +  "°C";
        var x = targetTemperatureObject.transform.localPosition.x;
        var z = targetTemperatureObject.transform.localPosition.z;
        var y = TemperatureData.target * 100 / temperatureSlider.maxValue;
        targetTemperatureObject.transform.localPosition = new Vector3(x, y, z);
        targetTemperatureText.text = "Target" + TemperatureData.target.ToString("0.0") + "°C";
    }
}
