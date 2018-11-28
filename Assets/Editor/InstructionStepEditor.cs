using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InstructionStepEditor : EditorWindow
{
    private bool initialized = false;
    private string objectName;

    internal InstructionStep PreviousStep { private get; set; }

    internal InstructionEditor Caller { private get; set; }

    private void OnGUI()
    {
        if (!initialized)
        {
            Initialize();
        }
        objectName = EditorGUILayout.TextField("Name", objectName);

        if (initialized)
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
        if (PreviousStep.Step != null)
        {
            objectName = PreviousStep.Step.name;
            initialized = true;
        }
    }

    private void CreateStep()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/InstructionStep.prefab");
        var instructionStep = new InstructionStep(Instantiate(prefab), null);
        instructionStep.Step.name = objectName;
        Caller.CreateStep(instructionStep);
        Caller.Focus();
        Close();
    }

    private void EditStep()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/InstructionStep.prefab");
        var instructionStep = new InstructionStep(Instantiate(prefab), null);
        instructionStep.Step.name = objectName;
        Caller.ReplaceStep(PreviousStep, instructionStep);
        Caller.Focus();
        Close();
    }

    private void OnLostFocus()
    {
        Focus();
    }
}

