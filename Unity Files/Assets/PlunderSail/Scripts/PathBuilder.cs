using UnityEditor;
using System.Collections;
using UnityEngine;

[CustomEditor(typeof(WayPointPath))]
public class PathBuilder : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WayPointPath myScript = (WayPointPath)target;
        
        if (GUILayout.Button("New Path"))
        {
            myScript.NewPath();
        }

        if (GUILayout.Button("Add Waypoint"))
        {
            myScript.AddWayPoint();
        }

        if (GUILayout.Button("Delete Path"))
        {
            myScript.DeletePath();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}