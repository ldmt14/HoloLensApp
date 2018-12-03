using System;
using UnityEngine;

[Serializable]
public class InstructionStepComponent
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public GameObject Prefab;
    public bool FoldoutStatus = false;

    public InstructionStepComponent(Transform transform, GameObject prefab)
    {
        Position = transform.localPosition;
        Rotation = transform.localRotation;
        Scale = transform.localScale;
        Prefab = prefab;
    }

    public InstructionStepComponent(GameObject prefab): this(prefab.transform, prefab)
    {
        
    }
}
