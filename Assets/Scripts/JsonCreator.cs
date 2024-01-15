using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonCreator : JsonEditor
{
    protected List<JsonData> newTemplates = new List<JsonData>();
    protected int currentTemplateIndex = 0;
    protected Vector2 scrollPosition;

    private string filename = "File Name";

    [MenuItem("Window/JsonEditor/Create JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonCreator>("Create JSON");
    }

    protected override void OnGUI()
    {
        if (newTemplates.Count <= 0)
        {
            newTemplates.Add(new JsonData());
        }

        GUILayout.Label("JSON Editor", EditorStyles.boldLabel);
        filename = EditorGUILayout.TextField("Filename", filename);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        bool entryDeleted = false;

        for (int i = 0; i < newTemplates.Count; i++)
        {
            JsonData currentTemplate = newTemplates[i];

            // Add a dark light color shade to separate entries
            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            EditorGUILayout.BeginVertical("box");
            GUI.backgroundColor = Color.white;

            GUILayout.Label($"Entry {i + 1}", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            //currentTemplate.gameObjectType = EditorGUILayout.TextField("GameObject Type", currentTemplate.gameObjectType);
            currentTemplate.gameObjectType = ChooseGameObjectType();
            if (GUILayout.Button("Delete This Entry"))
            {
                try
                {
                    newTemplates.RemoveAt(i);
                    entryDeleted = true;
                    currentTemplateIndex = Mathf.Max(0, i - 1);
                }
                catch (Exception)
                {
                    Debug.Log("Cannot delete the last entry.");
                }
            }
            GUILayout.EndHorizontal();
            if (entryDeleted)
            {
                EditorGUILayout.EndVertical();
                break;
            }

            currentTemplate.name = EditorGUILayout.TextField("Name", currentTemplate.name);
            currentTemplate.parent = EditorGUILayout.TextField("Parent", currentTemplate.parent);
            currentTemplate.position = EditorGUILayout.Vector3Field("Position", currentTemplate.position);
            currentTemplate.rotation = EditorGUILayout.Vector3Field("Rotation", currentTemplate.rotation);
            currentTemplate.scale = EditorGUILayout.Vector3Field("Scale", currentTemplate.scale);
            currentTemplate.color = EditorGUILayout.ColorField("Color", currentTemplate.color);

            EditorGUILayout.EndVertical();

        }

        EditorGUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Actions", EditorStyles.boldLabel);
        if (GUILayout.Button("Add New Entry"))
        {
            newTemplates.Add(new JsonData());
        }

        if (GUILayout.Button("Add Child"))
        {
            if (currentTemplateIndex < 3)
            {
                JsonData child = new JsonData { parent = newTemplates[currentTemplateIndex].name };
                newTemplates.Add(child);
                currentTemplateIndex++;
            }
            else
            {
                Debug.Log("Cannot add more than 3 levels of hierarchy.");
            }
        }

        if (GUILayout.Button("Add Parent"))
        {
            JsonData parent = new JsonData();
            newTemplates.Insert(0, parent);
            currentTemplateIndex = 0;
        }

        if (GUILayout.Button("Delete All Entries"))
        {
            newTemplates.Clear();
            newTemplates.Add(new JsonData());
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create JSON"))
        {
            CreateJsonData();
        }
        if (GUILayout.Button("Load JSON"))
        {
            LoadJson();
            //JsonLoader jsonLoader = new();
            //jsonLoader.LoadJsonData();
        }
        if (GUILayout.Button("Update JSON"))
        {
            CreateJsonData();
        }
        GUILayout.EndHorizontal();
    }

    private void LoadJson()
    {
        // Read JSON file and add entries to based on the JSON data
        string path = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
        if (path.Length != 0)
        {
            string jsonString = File.ReadAllText(path);
            JsonDataList templateList = JsonUtility.FromJson<JsonDataList>(jsonString);

            // Clear the current templates
            newTemplates.Clear();

            // Add loaded templates to newTemplates
            foreach (JsonData template in templateList.data)
            {
                newTemplates.Add(template);
            }
        }
    }

    private string ChooseGameObjectType()
    {
        List<string> options = new() { "Canvas", "Button", "Text", "Image" };
        try
        {
            int index = options.IndexOf(newTemplates[currentTemplateIndex].gameObjectType);
            int newIndex = EditorGUILayout.Popup("GameObject Type", index, options.ToArray());
            if (newIndex < 0)
            {
                newIndex = 0;
            }
            return options[newIndex];
        }
        catch (Exception)
        {
            return options[1];
        }
    }

    private void CreateJsonData()
    {
        string path = EditorUtility.SaveFilePanel("Create JSON File", "", filename, "json");
        if (path.Length != 0)
        {
            JsonDataList jsonDataList = new JsonDataList { data = newTemplates };
            string jsonString = JsonUtility.ToJson(jsonDataList);
            File.WriteAllText(path, jsonString);
        }
    }
}
