using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonCreator : JsonEditor
{
    protected List<JsonData> newTemplates = new List<JsonData>();
    protected int currentTemplateIndex = 0;
    protected Vector2 scrollPosition;

    [MenuItem("Window/JsonEditor/Create JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonCreator>("Create JSON");
    }

    protected override void OnGUI()
    {
        if (newTemplates.Count == 0)
        {
            newTemplates.Add(new JsonData());
        }

        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);

        // Start a scroll view
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < newTemplates.Count; i++)
        {
            JsonData currentTemplate = newTemplates[i];
            currentTemplate.name = EditorGUILayout.TextField("Name", currentTemplate.name);
            currentTemplate.position = EditorGUILayout.Vector3Field("Position", currentTemplate.position);
            currentTemplate.rotation = EditorGUILayout.Vector3Field("Rotation", currentTemplate.rotation);
            currentTemplate.scale = EditorGUILayout.Vector3Field("Scale", currentTemplate.scale);
            currentTemplate.color = EditorGUILayout.ColorField("Color", currentTemplate.color);

            // Add a button to delete this entry
            if (GUILayout.Button("Delete This Entry"))
            {
                newTemplates.RemoveAt(i);
                break;  // Break the loop because the list has been modified
            }

            EditorGUILayout.Space();  // Add some space between entries
        }

        // End the scroll view
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add New Entry"))
        {
            newTemplates.Add(new JsonData());
        }

        if (GUILayout.Button("Create JSON Data"))
        {
            CreateJsonData();
        }
    }

    private void CreateJsonData()
    {
        string path = EditorUtility.SaveFilePanel("Create JSON File", "", "", "json");
        if (path.Length != 0)
        {
            JsonDataList jsonDataList = new JsonDataList { data = newTemplates };
            string jsonString = JsonUtility.ToJson(jsonDataList);
            File.WriteAllText(path, jsonString);
        }
    }
}
