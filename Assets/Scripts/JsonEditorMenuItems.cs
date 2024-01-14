using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateJsonEditor : JsonEditor
{
    //Create JSON data
    public void CreateJsonData()
    {
        string path = EditorUtility.SaveFilePanel("Create JSON File", "", newTemplate.name, "json");
        if (path.Length != 0)
        {
            string jsonString = JsonUtility.ToJson(newTemplate);
            File.WriteAllText(path, jsonString);
        }
    }
}

public class LoadJsonEditor : JsonEditor
{
    //Load JSON data
    public void LoadJsonData()
    {
        string path = GetJsonFilePath();
        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            newTemplate = JsonUtility.FromJson<JsonData>(jsonString);
        }
    }
}

public class UpdateJsonEditor : JsonEditor
{
    //Edit JSON data
    public void UpdateJsonData()
    {
        CreateJsonEditor createJsonEditor = new();
        createJsonEditor.CreateJsonData();
    }
}

public class DeleteJsonEditor : JsonEditor
{
    //Delete JSON data
    public void DeleteJsonData()
    {
        string path = GetJsonFilePath();
        if (path.Length != 0 && File.Exists(path))
        {
            File.Delete(path);
        }
    }
}

public class InstantiateJsonEditor : JsonEditor
{
    public void InstantiateGameObject()
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
}

