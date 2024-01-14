using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonData
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public Color color;
    //public List<string> componentTypes;
    public List<ComponentData> components;
    //public List<Dictionary<string, object>> componentProperties;
}

[System.Serializable]
public class ComponentData
{
    public string type;
    public Dictionary<string, string> properties;
}