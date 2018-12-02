using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class VideoEditor : EditorWindow {
    private VideoClip videoClip;
    private int selection = 0;
    private string url;
    private string[] options = {"URL", "Video Clip"};

    private void OnGUI()
    {
        if ((selection = GUILayout.SelectionGrid(selection, options, options.Length)) == 0)
        {
            url = EditorGUILayout.TextField("URL", url);
        }
        else
        {
            videoClip = (VideoClip)EditorGUILayout.ObjectField("Video Clip", videoClip, typeof(VideoClip), true);
        }

        if (GUILayout.Button("Create Help"))
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI-Video.prefab");
            var videoObject = Instantiate(prefab);

            if (selection == 0)
            {
                videoObject.GetComponentInChildren<VideoPlayer>().url = url;
            }
            else
            {
                videoObject.GetComponentInChildren<VideoPlayer>().clip = videoClip;
            }

            videoObject.name = prefab.name;
            videoObject.GetComponent<Canvas>().worldCamera = Camera.main;
            GetWindow<InstructionStepEditor>().HelpObject = videoObject;
            Close();
        }
    }
}
