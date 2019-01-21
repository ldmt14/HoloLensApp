using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionObject : InspectableObject {

    public InstructionStep[] stepList;
    private int currentIndex = 0;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button closeButton;

    // Use this for initialization
    void Awake()
    {   
        currentIndex = 0;
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(NextStep);
        }
        if (backButton != null)
        {
            backButton.onClick.AddListener(PreviousStep);
        }
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }
        DisplayCurrentStep();
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void OnLeftSwipe()
    {
        NextStep();
    }

    public void OnRightSwipe()
    {
        PreviousStep();
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
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(currentIndex != stepList.Length - 1);
        }
        if (backButton != null)
        {
            backButton.gameObject.SetActive(currentIndex != 0);
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
