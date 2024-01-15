using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

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
    protected static void InstantiateGameObject()
    {
        string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");

        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            JsonDataList templateList = JsonUtility.FromJson<JsonDataList>(jsonString);

            Dictionary<string, GameObject> nameToObjectMap = new Dictionary<string, GameObject>();
            GameObject canvas = GameObject.FindObjectOfType<Canvas>()?.gameObject;

            foreach (JsonData template in templateList.data)
            {
                GameObject obj = CreateGameObject(template, ref canvas);
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

    private static GameObject CreateGameObject(JsonData template, ref GameObject canvas)
    {
        GameObject obj = new GameObject(template.name);

        switch (template.gameObjectType)
        {
            case "Canvas":
                obj.AddComponent<CanvasScaler>();
                obj.AddComponent<GraphicRaycaster>();
                break;
            case "Button":
                if (canvas == null)
                {
                    canvas = new GameObject("Canvas", typeof(Canvas));
                }
                // Set the parent of the button to the canvas
                obj.transform.SetParent(canvas.transform);
                obj.AddComponent<Button>();
                obj.AddComponent<Image>();
                // Set obj.AddComponent<TMPro.TextMeshPro>(); as child of the button
                GameObject textObj = new("Text", typeof(TextMeshProUGUI));
                textObj.GetComponent<TextMeshProUGUI>().text = template.name;
                textObj.transform.SetParent(obj.transform);
                break;
            case "Text":
                if (canvas == null)
                {
                    canvas = new GameObject("Canvas", typeof(Canvas));
                }
                // Set the parent of the button to the canvas
                obj.transform.SetParent(canvas.transform);
                obj.AddComponent<TextMeshProUGUI>();
                break;
            case "Image":
                if (canvas == null)
                {
                    canvas = new GameObject("Canvas", typeof(Canvas));
                }
                // Set the parent of the button to the canvas
                obj.transform.SetParent(canvas.transform);
                obj.AddComponent<Image>();
                break;
            default:
                break;
        }

        obj.transform.position = template.position;
        obj.transform.eulerAngles = template.rotation;
        obj.transform.localScale = template.scale;

        if (obj.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material.color = template.color;
        }

        return obj;
    }

}