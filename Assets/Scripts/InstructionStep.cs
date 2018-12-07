using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InstructionStep
{
    public GameObject Step;
    public GameObject Help;

    public InstructionStep(GameObject Step, GameObject Help)
    {
        this.Step = Step;
        this.Help = Help;
    }

    public InstructionStep Clone()
    {
        InstructionStep result;
        if (Help == null)
        {
            result = new InstructionStep(GameObject.Instantiate(Step), null);
            result.Step.name = Step.name;
        }
        else
        {
            result = new InstructionStep(GameObject.Instantiate(Step), GameObject.Instantiate(Step));
            result.Step.name = Step.name;
            result.Help.name = Help.name;
        }
        return result;
    }
}