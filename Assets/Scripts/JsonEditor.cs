using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonEditor : EditorWindow
{
    private JsonData newTemplate = new();

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

        if (GUILayout.Button("Instantiate Game Object"))
        {
            InstantiateGameObject();
        }
    }

    private void InstantiateGameObject()
    {
        // Assuming you have a method to get the path of the JSON file
        string path = GetJsonFilePath();

        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            JsonData template = JsonUtility.FromJson<JsonData>(jsonString);

            GameObject obj = new(template.name);
            obj.transform.position = template.position;
            obj.transform.eulerAngles = template.rotation;
            obj.transform.localScale = template.scale;

            // If your JsonData class includes color and you want to apply it to a Renderer
            if (obj.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.material.color = template.color;
            }
        }
    }

    private string GetJsonFilePath()
    {
        return EditorUtility.OpenFilePanel("Select JSON File", "", "json");
    }

    //Load JSON data
    private void LoadJsonData()
    {
        string path = GetJsonFilePath();
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
        string path = GetJsonFilePath();
        if (path.Length != 0 && File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
