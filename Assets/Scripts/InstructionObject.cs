using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionObject : InspectableObject {

    public InstructionStep[] stepList;
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
        if (currentIndex < stepList.Length - 1)
        {
            currentIndex++;
            DisplayCurrentStep();
        }
    }

    internal void PreviousStep()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            DisplayCurrentStep();
        }
    }

    void DisplayCurrentStep()
    {
        for (int i = 0; i < stepList.Length; i++)
        {
            stepList[i].Step.SetActive(i == currentIndex);
        }
    }

    public override void Help()
    {
        if (stepList[currentIndex].Help != null)
        {
            stepList[currentIndex].Help.SetActive(true);
        }
    }
}
