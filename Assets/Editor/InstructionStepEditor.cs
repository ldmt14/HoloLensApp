using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InstructionStepEditor : EditorWindow
{
    internal bool initialized = false;
    internal bool editMode;
    private string objectName;
    private GameObject helpObject;

    internal InstructionStep PreviousStep { get; set; }

    internal InstructionEditor Caller { private get; set; }

    private void OnGUI()
    {
        if (!initialized)
        {
            Initialize();
        }
        objectName = EditorGUILayout.TextField("Name", objectName);

        EditorGUILayout.BeginHorizontal();
        helpObject = (GameObject) EditorGUILayout.ObjectField("Help Object", helpObject, typeof(GameObject), true);
        if (GUILayout.Button("New Text"))
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI-Text.prefab");
            helpObject = Instantiate(prefab);
            helpObject.name = "UI-Text";
            var canvas = helpObject.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
        if (GUILayout.Button("New Video"))
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI-Video.prefab");
            helpObject = Instantiate(prefab);
            helpObject.name = "UI-Video";
            var canvas = helpObject.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
        EditorGUILayout.EndHorizontal();

        if (editMode)
        {
            if (GUILayout.Button("Edit Step"))
            {
                EditStep();
            }
        }
        else
        {
            if (GUILayout.Button("Create Step"))
            {
                CreateStep();
            }
        }
    }

    private void Initialize()
    {
        objectName = PreviousStep.Step != null ? PreviousStep.Step.name : "";
        helpObject = PreviousStep.Help;
        initialized = true;
    }

    private void CreateStep()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/InstructionStep.prefab");
        var instructionStep = new InstructionStep(Instantiate(prefab), helpObject);
        instructionStep.Step.name = objectName;
        if (helpObject != null)
        {
            helpObject.transform.parent = instructionStep.Step.transform;
        }
        Caller.CreateStep(instructionStep);
        Caller.Focus();
        Close();
    }

    private void EditStep()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/InstructionStep.prefab");
        var instructionStep = new InstructionStep(Instantiate(prefab), helpObject);
        instructionStep.Step.name = objectName;
        if (helpObject != null)
        {
            helpObject.transform.parent = instructionStep.Step.transform;
        }
        Caller.ReplaceStep(PreviousStep, instructionStep);
        Caller.Focus();
        Close();
    }
}

