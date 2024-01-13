using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JsonEditor : EditorWindow
{
    [MenuItem("Window/JSON Editor")]
    public static void ShowWindow()
    {
        GetWindow<JsonEditor>("JSON Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("This is my custom editor window!", EditorStyles.boldLabel);
    }

}
