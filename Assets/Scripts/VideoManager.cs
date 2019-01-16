using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour {

    private static VideoPlayer lastFocusedVideo;

    void Update()
    {
        var mainCamera = Camera.main.transform;
        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hitInfo, 30.0f))
        {
            var videoObject = hitInfo.collider.gameObject.GetComponentInChildren<VideoPlayer>();
            if (videoObject != null)
            {
                lastFocusedVideo = videoObject;
            }
        }
    }

    public static void Play()
    {
        if (lastFocusedVideo != null && lastFocusedVideo.enabled)
        {
            lastFocusedVideo.Play();
        }
    }

    public static void Pause()
    {
        if (lastFocusedVideo != null && lastFocusedVideo.enabled)
        {
            lastFocusedVideo.Pause();
        }
    }

    public static void Stop()
    {
        if (lastFocusedVideo != null && lastFocusedVideo.enabled)
        {
            lastFocusedVideo.Stop();
            lastFocusedVideo.gameObject.SetActive(false);
        }
    }
}
