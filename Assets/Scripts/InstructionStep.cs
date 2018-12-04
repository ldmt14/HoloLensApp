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
}
