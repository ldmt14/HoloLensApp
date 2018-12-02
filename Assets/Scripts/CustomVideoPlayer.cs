using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CustomVideoPlayer : MonoBehaviour {

    private VideoPlayer videoPlayerComponent;

    private void Awake()
    {
        videoPlayerComponent = gameObject.GetComponent<VideoPlayer>();
        if (videoPlayerComponent == null)
        {
            Debug.Log("Could not find VideoPlayer-Component");
        }
    }

    private void OnEnable()
    {
        videoPlayerComponent.frame = 0;
        videoPlayerComponent.Play();
    }

    private void OnDisable()
    {
        videoPlayerComponent.Pause();
    }
}
