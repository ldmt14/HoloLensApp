using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerEvent : UnityEvent<GameObject> { }

public class InspectableObject : MonoBehaviour{
    [SerializeField]
    private GameObject helpObject;
    [SerializeField]
    public PlayerEvent OnClose;

    public void Close()
    {
        OnClose.Invoke(gameObject);
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
