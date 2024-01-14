using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonEditor : EditorWindow
{
    protected JsonData newTemplate = new();

    protected string GetJsonFilePath()
    {
        return EditorUtility.OpenFilePanel("Select JSON File", "", "json");
    }

    protected virtual void OnGUI()
    {
        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);

        // Create input fields for each property of JsonData
        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
        newTemplate.position = EditorGUILayout.Vector3Field("Position", newTemplate.position);
        newTemplate.rotation = EditorGUILayout.Vector3Field("Rotation", newTemplate.rotation);
        newTemplate.scale = EditorGUILayout.Vector3Field("Scale", newTemplate.scale);
        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);
    }

    [MenuItem("Window/JsonEditor/Instiante Objects")]
    //protected static void InstantiateGameObject()
    //{
    //    // Assuming you have a method to get the path of the JSON file
    //    string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");

    //    if (path.Length != 0)
    //    {
    //        string jsonString = File.ReadAllText(path);
    //        JsonDataList templateList = JsonUtility.FromJson<JsonDataList>(jsonString);

    //        foreach (JsonData template in templateList.data)
    //        {
    //            GameObject obj = new(template.name);
    //            obj.transform.position = template.position;
    //            obj.transform.eulerAngles = template.rotation;
    //            obj.transform.localScale = template.scale;

    //            // If your JsonData class includes color and you want to apply it to a Renderer
    //            if (obj.TryGetComponent<Renderer>(out var renderer))
    //            {
    //                renderer.material.color = template.color;
    //            }
    //        }
    //    }
    //}
    protected static void InstantiateGameObject()
    {
        string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");

        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            JsonDataList templateList = JsonUtility.FromJson<JsonDataList>(jsonString);

            Dictionary<string, GameObject> nameToObjectMap = new Dictionary<string, GameObject>();

            foreach (JsonData template in templateList.data)
            {
                GameObject obj = new GameObject(template.name);
                obj.transform.position = template.position;
                obj.transform.eulerAngles = template.rotation;
                obj.transform.localScale = template.scale;

                if (obj.TryGetComponent<Renderer>(out var renderer))
                {
                    renderer.material.color = template.color;
                }

                nameToObjectMap[template.name] = obj;
            }

            foreach (JsonData template in templateList.data)
            {
                if (template.parent != null && nameToObjectMap.ContainsKey(template.parent))
                {
                    GameObject parentObj = nameToObjectMap[template.parent];
                    nameToObjectMap[template.name].transform.SetParent(parentObj.transform);
                }
            }
        }
    }

}