using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonEditor : EditorWindow
{
    private List<JsonData> newTemplates = new List<JsonData>();
    private string fileName = "JsonData";
    private Vector2 scrollPosition;

    [MenuItem("Window/JSON Editor")]
    public static void ShowWindow()
    {
        GetWindow<JsonEditor>("JSON Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("File Name", fileName);

        if (GUILayout.Button("New Template"))
        {
            newTemplates.Add(new JsonData());
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        try
        {
            for (int i = 0; i < newTemplates.Count; i++)
            {
                JsonData template = newTemplates[i];
                template.name = EditorGUILayout.TextField("Name", template.name);
                template.position = EditorGUILayout.Vector3Field("Position", template.position);
                template.rotation = EditorGUILayout.Vector3Field("Rotation", template.rotation);
                template.scale = EditorGUILayout.Vector3Field("Scale", template.scale);
                template.color = EditorGUILayout.ColorField("Color", template.color);

                if (GUILayout.Button("Delete Template"))
                {
                    newTemplates.RemoveAt(i);
                    return; // Exit the function to avoid an out-of-range exception
                }
            }
        }
        finally
        {
            GUILayout.EndScrollView();
        }

        if (GUILayout.Button("Load JSON Data"))
        {
            LoadJsonData();
        }

        if (GUILayout.Button("Save JSON Data"))
        {
            SaveJsonData();
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
            JsonDataList jsonDataList = JsonUtility.FromJson<JsonDataList>(jsonString);
            newTemplates = jsonDataList.data;
        }
    }

    //Save JSON data
    private void SaveJsonData()
    {
        string path = EditorUtility.SaveFilePanel("Save JSON File", "", fileName, "json");
        if (path.Length != 0)
        {
            Debug.Log("newTemplates count: " + newTemplates.Count); // Add this line
            string jsonString = JsonUtility.ToJson(newTemplates, true);
            Debug.Log("jsonString: " + jsonString); // Add this line
            File.WriteAllText(path, jsonString);
        }
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
