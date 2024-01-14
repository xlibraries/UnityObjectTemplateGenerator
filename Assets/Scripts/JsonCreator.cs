using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JsonCreator : JsonEditor
{
    [MenuItem("Window/JsonEditor/Create JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonCreator>("Create JSON");
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        if (GUILayout.Button("Create JSON Data"))
        {
            CreateJsonData();
        }
    }

    //private void CreateJsonData()
    //{
    //    string path = EditorUtility.SaveFilePanel("Create JSON File", "", newTemplate.name, "json");
    //    if (path.Length != 0)
    //    {
    //        // Set the componentTypes and componentProperties fields
    //        if (gameObject != null)
    //        {
    //            Component[] components = gameObject.GetComponents<Component>();
    //            newTemplate.componentTypes = new List<string>();
    //            newTemplate.componentProperties = new List<Dictionary<string, object>>();
    //            foreach (var component in components)
    //            {
    //                // Check if the component is a UI component
    //                if (component is UnityEngine.UI.Graphic || component is UnityEngine.UI.Selectable)
    //                {
    //                    newTemplate.componentTypes.Add(component.GetType().AssemblyQualifiedName);

    //                    // Store the properties of the component in a dictionary
    //                    //var properties = new Dictionary<string, object>();
    //                    //foreach (var property in component.GetType().GetProperties())
    //                    //{
    //                    //    try
    //                    //    {
    //                    //        //properties.Add(property.Name, property.GetValue(component));

    //                    //        if (property.CanRead)
    //                    //        {
    //                    //            properties.Add(property.Name, property.GetValue(component));
    //                    //        }
    //                    //    }
    //                    //    catch (System.Exception)
    //                    //    {
    //                    //        // Ignore the exception
    //                    //        continue;
    //                    //    }
    //                    //}
    //                    //newTemplate.componentProperties.Add(properties);
    //                }
    //            }
    //        }

    //        string jsonString = JsonUtility.ToJson(newTemplate);
    //        File.WriteAllText(path, jsonString);
    //    }
    //}

    private void CreateJsonData()
    {
        string path = EditorUtility.SaveFilePanel("Create JSON File", "", gameObject.name, "json");
        if (path.Length != 0)
        {
            JsonData newTemplate = new()
            {
                name = gameObject.name,
                components = new List<ComponentData>()
            };

            foreach (var component in gameObject.GetComponents<Component>())
            {
                ComponentData componentData = new()
                {
                    type = component.GetType().Name,
                    properties = new Dictionary<string, string>()
                };

                foreach (var property in component.GetType().GetProperties())
                {
                    if (property.CanRead)
                    {
                        try
                        {
                            var value = property.GetValue(component);
                            if (value != null)
                            {
                                componentData.properties.Add(property.Name, value.ToString());
                            }
                        }
                        catch (System.Exception)
                        {
                            // Ignore the exception and continue with the next property
                        }
                    }
                }
                newTemplate.components.Add(componentData);
            }

            string jsonString = JsonUtility.ToJson(newTemplate);
            File.WriteAllText(path, jsonString);
        }
    }

}
