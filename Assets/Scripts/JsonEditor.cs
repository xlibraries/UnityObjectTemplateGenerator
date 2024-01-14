using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonEditor : EditorWindow
{
    protected JsonData newTemplate = new();

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
            //LoadJsonEditor loadJsonEditor = new();
            //loadJsonEditor.LoadJsonData();
            ShowLoadWindow();
        }

        if (GUILayout.Button("Create JSON Data"))
        {
            //CreateJsonEditor createJsonEditor = new();
            //createJsonEditor.CreateJsonData();
            ShowCreateWindow();
        }

        if (GUILayout.Button("Update JSON Data"))
        {
            //UpdateJsonEditor updateJsonEditor = new();
            //updateJsonEditor.UpdateJsonData();
            ShowUpdateWindow();
        }

        if (GUILayout.Button("Delete JSON Data"))
        {
            //DeleteJsonEditor deleteJsonEditor = new();
            //deleteJsonEditor.DeleteJsonData();
            ShowDeleteWindow();
        }

        if (GUILayout.Button("Instantiate Game Object"))
        {
            //InstantiateJsonEditor instantiateJsonEditor = new();
            //instantiateJsonEditor.InstantiateGameObject();
            ShowInstantiateWindow();
        }
    }
    protected string GetJsonFilePath()
    {
        return EditorUtility.OpenFilePanel("Select JSON File", "", "json");
    }

    [MenuItem("Window/JSON Editor/Create")]
    public static void ShowCreateWindow()
    {
        GetWindow<CreateJsonEditor>("Create JSON");
    }

    [MenuItem("Window/JSON Editor/Load")]
    public static void ShowLoadWindow()
    {
        LoadJsonEditor loadJsonEditor = new();
        loadJsonEditor.LoadJsonData();
    }

    [MenuItem("Window/JSON Editor/Update")]
    public static void ShowUpdateWindow()
    {
        GetWindow<UpdateJsonEditor>("Update JSON");
    }

    [MenuItem("Window/JSON Editor/Delete")]
    public static void ShowDeleteWindow()
    {
        GetWindow<DeleteJsonEditor>("Delete JSON");
    }

    [MenuItem("Window/JSON Editor/Instantiate")]
    public static void ShowInstantiateWindow()
    {
        GetWindow<InstantiateJsonEditor>("Instantiate JSON");
    }
}
