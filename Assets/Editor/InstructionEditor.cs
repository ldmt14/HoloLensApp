﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InstructionEditor : EditorWindow {
    private GameObject instructionObject;
    private string objectName = "InstructionObject";
    private List<InstructionStep> instructionSteps = new List<InstructionStep>();

    private Action todo = null;

    [MenuItem("Tools/Instruction Editor")]
    private static void CreateInstruction()
    {
        GetWindow<InstructionEditor>();
    }

    internal void OnGUI()
    {
        instructionObject.name = objectName = EditorGUILayout.TextField("Name", objectName);

        var iterator = instructionSteps.GetEnumerator();

        beforeIterate();

        foreach(InstructionStep step in new List<InstructionStep>(instructionSteps))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box(step.Step.name, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Edit", GUILayout.ExpandWidth(false)))
            {
                var window = GetWindow<InstructionStepEditor>();
                window.Caller = this;
                window.PreviousStep = step;
                window.editMode = true;
                window.initialized = false;
            }
            if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false)))
            {
                todo += () =>
                {
                    instructionSteps.Remove(step);
                    var windows = Resources.FindObjectsOfTypeAll<InstructionStepEditor>();
                    if (windows != null && windows.Length > 0 && windows[0].PreviousStep.Equals(step))
                    {
                        windows[0].Close();
                    }
                    DestroyImmediate(step.Step);
                };
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Step"))
        {
            var window = GetWindow<InstructionStepEditor>();
            window.Caller = this;
            window.PreviousStep = new InstructionStep(null, null);
            window.editMode = false;
            window.initialized = false;
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("OK")) {
            Selection.activeGameObject = instructionObject;
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            DestroyImmediate(instructionObject);
            Close();
        }
        GUILayout.EndHorizontal();
    }

    internal void ReplaceStep(InstructionStep previousStep, InstructionStep newStep)
    {
        todo += () =>
        {
            int index = instructionSteps.IndexOf(previousStep);
            if (index >= 0)
            {
                instructionSteps[index] = newStep;
                newStep.Step.transform.parent = instructionObject.transform;
                newStep.Step.transform.SetSiblingIndex(index);
                DestroyImmediate(previousStep.Step);
            } else
            {
                throw new Exception("You can't replace an element thats not there.");
            }
        };
    }

    internal void CreateStep(InstructionStep instructionStep)
    {
        todo += () =>
        {
            instructionSteps.Add(instructionStep);
            instructionStep.Step.transform.parent = instructionObject.transform;
        };
    }

    private void beforeIterate()
    {
        if (todo != null)
        {
            todo.Invoke();
            todo = null;
        }
        instructionSteps.RemoveAll((obj) => obj.Step == null);
        var instructionScript = instructionObject.GetComponent<InstructionObject>();
        instructionScript.stepList = instructionSteps.ToArray();
    }

    private void Awake()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Instruction.prefab");
        instructionObject = Instantiate(prefab);
        instructionObject.name = objectName;
    }
}