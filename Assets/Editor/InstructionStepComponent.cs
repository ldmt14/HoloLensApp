using UnityEngine;

public class InstructionStepComponent
{
    public GameObject GameObject;
    public bool FoldoutStatus = false;

    public InstructionStepComponent(GameObject gameObject)
    {
        GameObject = gameObject;
    }
}
