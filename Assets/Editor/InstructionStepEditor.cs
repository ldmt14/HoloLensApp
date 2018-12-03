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
    private List<InstructionStepComponent> components = new List<InstructionStepComponent>();
    private GameObject prefab;
    private Action todo;

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
        BeforeIterate();

        foreach(InstructionStepComponent component in components) {
            EditorGUILayout.BeginHorizontal();
            component.FoldoutStatus = EditorGUILayout.Foldout(component.FoldoutStatus, component.Prefab.name);
            if (GUILayout.Button("Remove"))
            {
                todo += () =>
                {
                    components.Remove(component);
                };
            }
            EditorGUILayout.EndHorizontal();

            if (component.FoldoutStatus)
            {
                EditorGUI.indentLevel++;
                component.Prefab = (GameObject)EditorGUILayout.ObjectField("GameObject", component.Prefab, typeof(GameObject), true);
                component.Position = EditorGUILayout.Vector3Field("Position", component.Position);
                component.Rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", component.Rotation.eulerAngles));
                component.Scale = EditorGUILayout.Vector3Field("Scale", component.Scale);
                EditorGUI.indentLevel--;
            }
        }
        
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab for new Component", prefab, typeof(GameObject), false);
        if (prefab != null && GUILayout.Button("New Component"))
        {
            components.Add(new InstructionStepComponent(prefab));
        }

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

    private void BeforeIterate()
    {
        if (todo != null)
        {
            todo.Invoke();
            todo = null;
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
        foreach (InstructionStepComponent component in components)
        {
            var obj = Instantiate(component.Prefab);
            obj.name = component.Prefab.name;
            obj.transform.parent = instructionStep.Step.transform;
            obj.transform.localPosition = component.Position;
            obj.transform.localRotation = component.Rotation;
            obj.transform.localScale = component.Scale;
        }
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

