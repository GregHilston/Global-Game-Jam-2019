using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapGenerator mapGenerator = (MapGenerator)target;
        if(GUILayout.Button("Generate New Map"))
        {
            mapGenerator.GenerateMap();
        }
    }
}
