using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RigPart))]
public class RigPartEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        RigPart rigPart = (RigPart)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("persistentLength"));
            
        if (!rigPart.persistentLength)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stretch"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("squash"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("elastic"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
