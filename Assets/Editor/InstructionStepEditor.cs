using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InstructionStepEditor : EditorWindow
{
    internal bool initialized = false;
    internal bool editMode;
    private List<InstructionStepComponent> components = new List<InstructionStepComponent>();
    private GameObject prefab;
    private Action todo;

    internal GameObject HelpObject { private get; set; }

    internal InstructionStep PreviousStep { get; set; }

    private InstructionStep newStep;

    internal InstructionEditor Caller { private get; set; }

    private void OnGUI()
    {
        if (!initialized)
        {
            Initialize();
        }
        newStep.Step.name = EditorGUILayout.TextField("Name", newStep.Step.name);
        BeforeIterate();

        foreach(InstructionStepComponent component in components) {
            EditorGUILayout.BeginHorizontal();
            component.FoldoutStatus = EditorGUILayout.Foldout(component.FoldoutStatus, component.GameObject.name);
            if (GUILayout.Button("Remove"))
            {
                todo += () =>
                {
                    components.Remove(component);
                    DestroyImmediate(component.GameObject);
                };
            }
            EditorGUILayout.EndHorizontal();

            if (component.FoldoutStatus)
            {
                EditorGUI.indentLevel++;
                component.GameObject = (GameObject)EditorGUILayout.ObjectField("GameObject", component.GameObject, typeof(GameObject), true);
                component.GameObject.transform.position = EditorGUILayout.Vector3Field("Position", component.GameObject.transform.position);
                component.GameObject.transform.localRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", component.GameObject.transform.localRotation.eulerAngles));
                component.GameObject.transform.localScale = EditorGUILayout.Vector3Field("Scale", component.GameObject.transform.localScale);
                EditorGUI.indentLevel--;
            }
        }
        
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab for new Component", prefab, typeof(GameObject), false);
        if (prefab != null && GUILayout.Button("New Component"))
        {
            AddComponent(prefab);
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

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("OK"))
        {
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Cancel();
            Close();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddComponent(GameObject prefab)
    {
        var obj = Instantiate(prefab);
        obj.name = prefab.name;
        obj.transform.SetParent(newStep.Step.transform, false);
        EditorGUIUtility.PingObject(obj);
        Selection.activeGameObject = obj;
        SceneView.lastActiveSceneView.FrameSelected();
        components.Add(new InstructionStepComponent(obj));
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
        if (PreviousStep.Step != null)
        {
            newStep = PreviousStep;
            foreach (Transform child in newStep.Step.transform)
            {
                //Skip the HelpObject
                if (newStep.Help != null && child == newStep.Help.transform)
                {
                    continue;
                }
                components.Add(new InstructionStepComponent(child.gameObject));
            }
        }
        else
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/InstructionStep.prefab");
            newStep = new InstructionStep(Instantiate(prefab), null);
            newStep.Step.name = prefab.name;
        }
        HelpObject = newStep.Help;
        initialized = true;
    }

    private void SubmitStep()
    {
        var instructionStep = new InstructionStep(newStep.Step, HelpObject);
        if (HelpObject != null)
        {
            HelpObject.transform.SetParent(instructionStep.Step.transform, false);
        }
        if (editMode)
        {
            Caller.ReplaceStep(PreviousStep, instructionStep);
        }
        else
        {
            Caller.CreateStep(instructionStep);
        }
        Caller.Focus();
    }

    private void Cancel()
    {
        DestroyImmediate(newStep.Step);
        DestroyImmediate(newStep.Help);
    }

    private void OnDestroy()
    {
        if (newStep.Step != null)
        {
            SubmitStep();
        }
    }
}

