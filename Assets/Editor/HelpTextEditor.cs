using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HelpTextEditor : EditorWindow {
    private string helpText;

    private void OnGUI()
    {
        helpText = EditorGUILayout.TextField("Help Text", helpText);

        if (GUILayout.Button("Create Help"))
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI-Text.prefab");
            var helpObject = Instantiate(prefab);

            var helpTextObject = helpObject.GetComponentsInChildren<Text>().Where(text => text.gameObject.name == "Content").FirstOrDefault();
            if (helpTextObject != null)
            {
                helpTextObject.text = helpText;
            }

            helpObject.name = prefab.name;
            helpObject.GetComponent<Canvas>().worldCamera = Camera.main;
            GetWindow<InstructionStepEditor>().HelpObject = helpObject;
            Close();
        }
    }
}
