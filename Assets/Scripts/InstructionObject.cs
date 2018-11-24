using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionObject : MonoBehaviour {

    public GameObject[] stepList;
    private int currentIndex = 0;

	// Use this for initialization
	void Awake()
    {   
        currentIndex = 0;
        DisplayCurrentStep();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    internal void NextStep()
    {
        if (currentIndex < stepList.Length)
        {
            currentIndex++;
            DisplayCurrentStep();
        }
    }

    internal void PreviousStep()
    {
        if (currentIndex >= 0)
        {
            currentIndex--;
            DisplayCurrentStep();
        }
    }

    void DisplayCurrentStep()
    {
        for (int i = 0; i < stepList.Length; i++)
        {
            stepList[i].SetActive(i == currentIndex);
        }
    }
}
