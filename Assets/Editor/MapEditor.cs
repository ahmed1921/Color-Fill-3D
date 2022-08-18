using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class MapEditor : Editor
{
    Level level;
    private int _arrayIndex=0;
    private void OnEnable()
    {
        level  = (Level) target;
    }

//Area 51
    public override void OnInspectorGUI()
    {      
        level._height = EditorGUILayout.IntField("Height", level._height);
        level._width = EditorGUILayout.IntField("width", level._width);
        SerializedObject so = new SerializedObject(target);
        serializedObject.Update();
        SerializedProperty _widthProperty = so.FindProperty("_tilesData");

        _widthProperty.arraySize = level._width * level._height;
        _arrayIndex = 0;
        for (int i=0;i<level._height;i++) {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            for (int j = 0; j < level._width; j++)
            {
                var prop = so.FindProperty(string.Format("{0}.Array.data[{1}]", "_tilesData",_arrayIndex));
                prop.intValue= EditorGUILayout.IntField( prop.intValue);
                so.ApplyModifiedProperties();
                _arrayIndex++;
            }                
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}