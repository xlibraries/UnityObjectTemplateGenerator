#region 08 - JSON Editor

//using System;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//public class JsonEditor : EditorWindow
//{
//    protected JsonData newTemplate = new();

//    protected string GetJsonFilePath()
//    {
//        return EditorUtility.OpenFilePanel("Select JSON File", "", "json");
//    }

//    protected virtual void OnGUI()
//    {
//        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);

//        // Create input fields for each property of JsonData
//        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
//        newTemplate.position = EditorGUILayout.Vector3Field("Position", newTemplate.position);
//        newTemplate.rotation = EditorGUILayout.Vector3Field("Rotation", newTemplate.rotation);
//        newTemplate.scale = EditorGUILayout.Vector3Field("Scale", newTemplate.scale);
//        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);
//    }

//    [MenuItem("Window/JsonEditor/Instiante Objects")]
//    protected static void InstantiateGameObject()
//    {
//        // Assuming you have a method to get the path of the JSON file
//        string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");

//        if (path.Length != 0)
//        {
//            string jsonString = File.ReadAllText(path);
//            JsonData template = JsonUtility.FromJson<JsonData>(jsonString);

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

#endregion

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class JsonEditor : EditorWindow
{
    protected JsonData newTemplate = new();
    public GameObject gameObject; // Field for dragging and dropping a GameObject

    // Static reference to the current JsonEditor window
    public static JsonEditor current;

    protected string GetJsonFilePath()
    {
        return EditorUtility.OpenFilePanel("Select JSON File", "", "json");
    }

    protected virtual void OnGUI()
    {
        // Set the current window
        current = this;

        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);

        // Field for dragging and dropping a GameObject
        gameObject = (GameObject)EditorGUILayout.ObjectField("Game Object", gameObject, typeof(GameObject), true);

        // Create input fields for each property of JsonData
        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
        newTemplate.position = EditorGUILayout.Vector3Field("Position", newTemplate.position);
        newTemplate.rotation = EditorGUILayout.Vector3Field("Rotation", newTemplate.rotation);
        newTemplate.scale = EditorGUILayout.Vector3Field("Scale", newTemplate.scale);
        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);
    }

    [MenuItem("Window/JsonEditor/Instantiate Objects")]
    protected static void InstantiateGameObject()
    {
        // Assuming you have a method to get the path of the JSON file
        string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");

        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            JsonData template = JsonUtility.FromJson<JsonData>(jsonString);

            if (template == null)
            {
                Debug.LogError("Invalid JSON data");
                return;
            }

            GameObject obj = new(template.name);
            obj.transform.position = template.position;
            obj.transform.eulerAngles = template.rotation;
            obj.transform.localScale = template.scale;

            foreach (var componentData in template.components)
            {
                // Add the component to the GameObject
                string assemblyName;
                switch (componentData.type)
                {
                    case "RectTransform":
                        assemblyName = "UnityEngine.CoreModule";
                        break;
                    case "CanvasRenderer":
                        assemblyName = "UnityEngine.UIModule";
                        break;
                    case "Image":
                        assemblyName = "UnityEngine.UI";
                        break;
                    default:
                        assemblyName = "UnityEngine";
                        break;
                }
                Type componentType = Type.GetType($"{componentData.type}, {assemblyName}");
                if (componentType != null)
                {
                    try
                    {
                        obj.AddComponent(componentType);
                    }
                    catch (Exception)
                    {
                        Debug.LogError($"Failed to add component {componentData.type}");
                        continue;
                    }
                }
                else
                {
                    Debug.LogError($"Component type {componentData.type} not found");
                }
            }


        }
    }

}

