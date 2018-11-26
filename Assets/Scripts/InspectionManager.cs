using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionManager : MonoBehaviour {

    private static InspectableObject lastFocusedObject;

    public static InspectableObject LastFocusedObject { get; private set; }
	
	// Update is called once per frame
	void Update ()
    {
        var mainCamera = Camera.main.transform;
        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hitInfo, 30.0f))
        {
            var inspectableObject = hitInfo.collider.gameObject.GetComponent<InspectableObject>();
            if (inspectableObject != null)
            {
                LastFocusedObject = inspectableObject;
            }
        }
    }

    public static void Help()
    {
        if (LastFocusedObject != null)
        {
            LastFocusedObject.Help();
        }
    }

    public static void Close()
    {
        if (LastFocusedObject != null)
        {
            LastFocusedObject.Close();
        }
    }
}
