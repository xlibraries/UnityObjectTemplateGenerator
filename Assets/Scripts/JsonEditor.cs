using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonEditor : EditorWindow
{
    private JsonData newTemplate = new JsonData();

    [MenuItem("Window/JSON Editor")]
    public static void ShowWindow()
    {
        GetWindow<JsonEditor>("JSON Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);

        // Create input fields for each property of JsonData
        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
        newTemplate.position = EditorGUILayout.Vector3Field("Position", newTemplate.position);
        newTemplate.rotation = EditorGUILayout.Vector3Field("Rotation", newTemplate.rotation);
        newTemplate.scale = EditorGUILayout.Vector3Field("Scale", newTemplate.scale);
        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);

        if (GUILayout.Button("Load JSON Data"))
        {
            LoadJsonData();
        }

        if (GUILayout.Button("Create JSON Data"))
        {
            CreateJsonData();
        }

        if (GUILayout.Button("Update JSON Data"))
        {
            UpdateJsonData();
        }

        if (GUILayout.Button("Delete JSON Data"))
        {
            DeleteJsonData();
        }
    }

    //Load JSON data
    private void LoadJsonData()
    {
        string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            newTemplate = JsonUtility.FromJson<JsonData>(jsonString);
        }
    }

    //Create JSON data
    private void CreateJsonData()
    {
        string path = EditorUtility.SaveFilePanel("Create JSON File", "", newTemplate.name, "json");
        if (path.Length != 0)
        {
            string jsonString = JsonUtility.ToJson(newTemplate);
            File.WriteAllText(path, jsonString);
        }
    }


    //Edit JSON data
    private void UpdateJsonData()
    {
        CreateJsonData();
    }

    //Delete JSON data
    private void DeleteJsonData()
    {
        string path = EditorUtility.OpenFilePanel("Select JSON File to Delete", "", "json");
        if (path.Length != 0 && File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
