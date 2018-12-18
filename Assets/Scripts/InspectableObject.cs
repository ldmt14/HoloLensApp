using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InspectableObject : MonoBehaviour{
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public abstract void Help();

    public virtual void OnSelect()
    {
        Help();
    }
}
