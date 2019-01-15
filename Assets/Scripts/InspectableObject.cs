using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectableObject : MonoBehaviour{
    [SerializeField]
    private GameObject helpObject;

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Help()
    {
        if (helpObject != null)
        {
            helpObject.SetActive(true);
        }
    }

    public virtual void OnSelect()
    {
        Help();
    }
}
