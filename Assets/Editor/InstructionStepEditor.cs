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
    internal GameObject HelpObject { private get; set; }

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
        HelpObject = (GameObject) EditorGUILayout.ObjectField("Help Object", HelpObject, typeof(GameObject), true);
        if (GUILayout.Button("New Text"))
        {
            GetWindow<HelpTextEditor>();
        }
        if (GUILayout.Button("New Video"))
        {
            GetWindow<VideoEditor>();
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
        HelpObject = PreviousStep.Help;
        initialized = true;
    }

    private void CreateStep()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/InstructionStep.prefab");
        var instructionStep = new InstructionStep(Instantiate(prefab), HelpObject);
        instructionStep.Step.name = objectName;
        if (HelpObject != null)
        {
            HelpObject.transform.parent = instructionStep.Step.transform;
        }
        Caller.CreateStep(instructionStep);
        Caller.Focus();
        Close();
    }

    private void EditStep()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/InstructionStep.prefab");
        var instructionStep = new InstructionStep(Instantiate(prefab), HelpObject);
        instructionStep.Step.name = objectName;
        if (HelpObject != null)
        {
            HelpObject.transform.parent = instructionStep.Step.transform;
        }
        Caller.ReplaceStep(PreviousStep, instructionStep);
        Caller.Focus();
        Close();
    }
}

