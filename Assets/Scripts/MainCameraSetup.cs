using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MainCameraSetup : MonoBehaviour {

#if UNITY_EDITOR

    // Update is called once per frame
    void Update()
    {
        var camera = GetComponent<Camera>();
        foreach (var canvas in Resources.FindObjectsOfTypeAll<Canvas>())
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = camera;
        }
    }
#endif
}
