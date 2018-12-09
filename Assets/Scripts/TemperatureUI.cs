using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OctoPi;
using System;
using UnityEngine.UI;

public class TemperatureUI : MonoBehaviour {
    private Slider temperatureSlider;
    private Text actualTemperatureText;
    private Text targetTemperatureText;
    private TemperatureData temperataureData;
    public TemperatureData TemperatureData {
        private get
        {
            return temperataureData;
        }
        set {
            temperataureData = value;
            UpdateTemperatureUI();
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateTemperatureUI()
    {
        temperatureSlider.value = TemperatureData.actual;
        actualTemperatureText.text = TemperatureData.actual.ToString("0.0");
    }
}
